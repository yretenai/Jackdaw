using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DragonLib;
using DragonLib.CommandLine;
using DragonLib.Hash;
using DragonLib.Hash.Basis;
using DragonLib.Platform;
using Jackdaw.Cache;
using Jackdaw.Structs.Client;
using Serilog;

namespace Jackdaw.ResourceCache;

internal class Program {
	private static readonly Uri APP_DOMAIN = new("https://binaries.eveonline.com", UriKind.Absolute);
	private static readonly Uri RES_DOMAIN = new("https://resources.eveonline.com", UriKind.Absolute);

	private static async Task Main() {
		Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Console().CreateLogger();

		var basicFlags = CommandLineFlagsParser.ParseFlags<ResCacheBasicFlags>(CommandLineFlagsParser.PrintHelpInvoker<ResCacheFlags>);
		if (basicFlags == null) {
			return;
		}

		if (basicFlags.Repair) {
			Repair(basicFlags);
			return;
		}

		var flags = CommandLineFlagsParser.ParseFlags<ResCacheFlags>();
		if (flags == null) {
			return;
		}

		var canMakeSymlinks = flags.Symlink || PlatformUtils.CanCreateSymlinks;
		if (flags.NoSymlink) {
			canMakeSymlinks = false;
		}

		var method = canMakeSymlinks ? "Linking" : "Copying";

		var cacheRoot = Path.GetFullPath(flags.ResCache);
		var outputPath = Path.GetFullPath(flags.Output);

		var indexFiles = flags.IndexFiles;
		if (indexFiles.Count == 2 && (new DirectoryInfo(indexFiles[0]).Attributes & FileAttributes.Directory) != 0) {
			var flyCatcherPath = indexFiles[0];
			var version = indexFiles[1];
			indexFiles.Clear();
			indexFiles.Add(Path.Combine(flyCatcherPath, "index", $"eveonline_{version}.txt.zst"));
			indexFiles.Add(Path.Combine(flyCatcherPath, "prefetch", $"resfileindex_prefetch_{version}.txt.zst"));
			indexFiles.Add(Path.Combine(flyCatcherPath, "resource", $"resfileindex_{version}.txt.zst"));
			indexFiles.Add(Path.Combine(flyCatcherPath, "windows", $"resfileindex_Windows_{version}.txt.zst"));
			indexFiles.Add(Path.Combine(flyCatcherPath, "index", $"eveonlinemacOS_{version}.txt.zst"));
			indexFiles.Add(Path.Combine(flyCatcherPath, "mac", $"resfileindex_macOS_{version}.txt.zst"));
		}

		// pass 1: delete directory
		if (Directory.Exists(outputPath) && flags.Clean) {
			Log.Information("Clearing existing install");
			if (!flags.Dry) {
				Directory.Delete(outputPath, true);
			}
		}

		var totalRecords = new List<ResourceCacheRecord>();

		foreach (var indexPath in indexFiles) {
			if (!File.Exists(indexPath)) {
				Log.Error("Index file {Path} does not exist", indexPath);
				continue;
			}

			ResourceCacheRecord[] records;
			if (indexPath.EndsWith(".zst", StringComparison.OrdinalIgnoreCase)) {
				await using var fs = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				using var data = JackdawUtils.Decompress(fs);
				records = IndexParser.Parse(data);
			} else {
				await using var fs = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				records = IndexParser.Parse(fs);
			}

			totalRecords.AddRange(records);
		}

		if (flags.Update) {
			var sourceCache = BuildCacheList(cacheRoot, outputPath, true);

			var lookup = totalRecords.DistinctBy(x => x.Path).ToDictionary(x => x.Path, x => x);

			Log.Information("Removing changed files");
			foreach (var record in sourceCache) {
				var prefix = "res";
				if (record.Path.Scheme == "app") {
					prefix = "";
				}

				var target = Path.Combine(outputPath, prefix, record.Path.AbsolutePath[1..]);
				if (!File.Exists(target)) {
					continue;
				}

				if (!lookup.TryGetValue(record.Path, out var existingRecord)) {
					Log.Information("Deleting {Path} as it is deleted", record.Path.AbsolutePath[1..]);
					if (!flags.Dry) {
						File.Delete(target);
					}

					continue;
				}

				if (existingRecord.MD5 != record.MD5 || existingRecord.Size != record.Size || existingRecord.ResourcePath != record.ResourcePath) {
					Log.Information("Deleting {Path} as it has changed", record.Path.AbsolutePath[1..]);
					if (!flags.Dry) {
						File.Delete(target);
					}
				}
			}
		}

		using var httpClient = CreateHttpClient();

		var md5Lookup = new Dictionary<string, string>();
		var current = 0;

		// pass 2: create directories
		Log.Information("Creating directories");
		foreach (var record in totalRecords.DistinctBy(x => Path.GetDirectoryName(x.Path.AbsolutePath[1..]))) {
			var prefix = "res";
			if (record.Path.Scheme == "app") {
				prefix = "";
			}

			var target = Path.Combine(outputPath, prefix, record.Path.AbsolutePath[1..]);
			var fullPath = Path.GetFullPath(target);
			var directory = Path.GetDirectoryName(fullPath);
			if (string.IsNullOrEmpty(directory)) {
				continue;
			}

			Log.Information("Creating directory {Path}", Path.GetRelativePath(outputPath, directory));
			if (flags.Dry) {
				continue;
			}

			Directory.CreateDirectory(directory);
		}

		// pass 3: actualize files
		Log.Information("Downloading files");
		foreach (var record in totalRecords) {
			var resPath = record.ResourcePath;

			if (!flags.NoDeduplication) {
				if (!md5Lookup.TryGetValue(record.MD5, out resPath)) {
					resPath = record.ResourcePath;
					md5Lookup.Add(record.MD5, resPath);
				}
			}

			var host = RES_DOMAIN;
			var prefix = "res";
			if (record.Path.Scheme == "app") {
				host = APP_DOMAIN;
				prefix = "";
			}

			var resFilePath = Path.Combine(cacheRoot, resPath);

			if (!File.Exists(resFilePath)) {
				try {
					Log.Information("Downloading {Path}", resPath);
					await Download(httpClient, flags, cacheRoot, resPath, host);
				} catch (Exception e) {
					Log.Error(e, "Failed to download {ResPath}", resPath);
					continue;
				}
			}

			Log.Information("[{Current}/{Total}/{Percent:F2}%] {Method} {Path}", ++current, totalRecords.Count, (float) current / totalRecords.Count * 100, method, record.Path.AbsolutePath[1..]);
			if (flags.Dry) {
				continue;
			}

			var target = Path.Combine(outputPath, prefix, record.Path.AbsolutePath[1..]);
			var info = new FileInfo(target);
			if (info.Exists) {
				var isSymlink = (info.Attributes & FileAttributes.ReparsePoint) != 0;

				var skipOverwrite = canMakeSymlinks switch {
					                    true when !isSymlink => false,
					                    false when isSymlink => false,
					                    _ => flags.NoOverwrite,
				                    };

				if (isSymlink &&
				    !string.IsNullOrEmpty(info.LinkTarget) &&
				    !File.Exists(Path.Combine(info.DirectoryName ?? outputPath, info.LinkTarget))) {
					skipOverwrite = false; // file is missing.
				}

				if (skipOverwrite) {
					continue;
				}

				File.Delete(target);
			}

			if (canMakeSymlinks) {
				File.CreateSymbolicLink(target, Path.GetRelativePath(Path.GetDirectoryName(target) ?? target, resFilePath));
			} else {
				File.Copy(resFilePath, target);
			}
		}

		if (flags.Reclaim) {
			// pass 4: clearing cache files
			Log.Information("Reclaiming storage");
			var cacheRepo = BuildCacheList(cacheRoot, outputPath).Select(x => Path.GetFileName(x.ResourcePath)).ToHashSet();
			cacheRepo.UnionWith(totalRecords.Select(x => Path.GetFileName(x.ResourcePath)));

			foreach (var file in Directory.EnumerateFiles(cacheRoot, "*", SearchOption.AllDirectories)) {
				var relative = Path.GetRelativePath(cacheRoot, file).Replace('\\', '/');
				if (relative.StartsWith('.') || cacheRepo.Contains(Path.GetFileName(relative))) {
					continue;
				}

				Log.Information("Deleting {File}", relative);
				if (flags.Dry) {
					continue;
				}

				File.Delete(file);
			}
		}

		WriteCacheList(cacheRoot, outputPath, indexFiles);
	}

	private static async Task Download(HttpClient client, ResCacheBasicFlags flags, string cacheRoot, string resPath, Uri host) {
		var path = Path.Combine(cacheRoot, resPath);
		if (flags is { Dry: false, NoDownload: false }) {
			path.EnsureDirectoryExists();
			await using var local = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			await using var remote = await client.GetStreamAsync(new Uri(host, resPath));
			await remote.CopyToAsync(local);
		}
	}

	private static HttpClient CreateHttpClient() {
		HttpClient? httpClient = null;
		try {
			var httpHandler = new HttpClientHandler();
			httpHandler.CheckCertificateRevocationList = true;
			httpHandler.AllowAutoRedirect = true;
			httpHandler.AutomaticDecompression = DecompressionMethods.All;

			httpClient = new HttpClient(httpHandler, true);
			httpClient.DefaultRequestHeaders.UserAgent.Clear();
			httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Jackdaw/1.0.0 (Module/ResCacheDownloader)");
			return httpClient;
		} catch {
			httpClient?.Dispose();
			throw;
		}
	}

	private static void Repair(ResCacheBasicFlags flags) {
		Log.Information("Repairing...");
		using var client = CreateHttpClient();

		Span<byte> buffer = stackalloc byte[0x20];
		var expectedHash = buffer[..0x10];
		var localHash = buffer[0x10..];

		foreach (var file in Directory.EnumerateFiles(flags.ResCache, "*", SearchOption.AllDirectories)) {
			var relative = Path.GetRelativePath(flags.ResCache, file).Replace('\\', '/');
			var underscore = relative.IndexOf('_', StringComparison.Ordinal);
			if (relative.StartsWith('.') || underscore == -1) {
				continue;
			}

			Convert.FromHexString(relative[(underscore + 1)..], expectedHash, out _, out _);
			using var stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
			MD5.HashData(stream, localHash);

			if (!expectedHash.SequenceEqual(localHash)) {
				Log.Information("{ResPath} Corrupt, replacing", relative);
				try {
					Download(client, flags, flags.ResCache, relative, RES_DOMAIN).Wait();
				} catch {
					try {
						Download(client, flags, flags.ResCache, relative, APP_DOMAIN).Wait();
					} catch (Exception e) {
						Log.Error(e, "Failed to download {ResPath}", relative);
					}
				}
			}
		}
	}

	private static void WriteCacheList(string cacheRoot, string outputPath, List<string> indexFiles) {
		var buffer = Array.Empty<byte>();

		var skipTarget = CRC.Create(CRC64Variants.Default).ComputeHashValue(Encoding.UTF8.GetBytes(outputPath)).ToString("x16");
		var storagePath = Path.Combine(cacheRoot, ".jackdaw", skipTarget);
		if (Directory.Exists(storagePath)) {
			Directory.Delete(storagePath, true);
		}

		Directory.CreateDirectory(storagePath);

		try {
			foreach (var indexPath in indexFiles) {
				using var stream = new FileStream(indexPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
				if (stream.Length > buffer.Length) {
					if (buffer.Length > 0) {
						ArrayPool<byte>.Shared.Return(buffer);
					}

					buffer = ArrayPool<byte>.Shared.Rent((int) stream.Length);
				}

				var ext = ".txt";
				if (indexPath.EndsWith(".zst", StringComparison.OrdinalIgnoreCase)) {
					ext += ".zst";
				}

				var block = buffer.AsSpan(0, (int) stream.Length);
				_ = stream.Read(block);
				var hash = CRC.Create(CRC64Variants.Default).ComputeHashValue(block).ToString("x16");
				using var target = new FileStream(Path.Combine(storagePath, hash + ext), FileMode.Create, FileAccess.Write);
				target.Write(block);
			}
		} finally {
			if (buffer.Length > 0) {
				ArrayPool<byte>.Shared.Return(buffer);
			}
		}
	}

	private static List<ResourceCacheRecord> BuildCacheList(string cacheRoot, string outputPath, bool onlySelf = false) {
		var skipTarget = CRC.Create(CRC64Variants.Default).ComputeHashValue(Encoding.UTF8.GetBytes(outputPath)).ToString("x16");
		var indexPath = Path.Combine(cacheRoot, ".jackdaw");
		Directory.CreateDirectory(indexPath);

		var records = new List<ResourceCacheRecord>();
		foreach (var txt in Directory.EnumerateFiles(indexPath, "*", SearchOption.AllDirectories)) {
			if (!txt.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) && !txt.EndsWith(".zst", StringComparison.OrdinalIgnoreCase)) {
				continue;
			}

			var isSelf = Path.GetFileName(Path.GetDirectoryName(txt)!) == skipTarget;

			switch (isSelf) {
				case true when !onlySelf:
				case false when onlySelf:
					continue;
			}

			if (txt.EndsWith(".zst", StringComparison.OrdinalIgnoreCase)) {
				using var fs = new FileStream(txt, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				using var data = JackdawUtils.Decompress(fs);
				records.AddRange(IndexParser.Parse(data));
			} else {
				using var fs = new FileStream(txt, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				records.AddRange(IndexParser.Parse(fs));
			}
		}

		return records;
	}
}
