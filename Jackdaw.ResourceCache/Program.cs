using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
		var flags = CommandLineFlagsParser.ParseFlags<ResCacheFlags>();
		if (flags == null) {
			return;
		}

		var canMakeSymlinks = flags.Symlink || PlatformUtils.CanCreateSymlinks;
		if (flags.NoSymlink) {
			canMakeSymlinks = false;
		}

		var method = canMakeSymlinks ? "Linking" : "Copying";

		Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Console().CreateLogger();

		var cacheRoot = Path.GetFullPath(flags.ResCache);
		var outputPath = Path.GetFullPath(flags.Output);

		var cacheRepo = BuildCacheList(cacheRoot, outputPath);
		var targetCache = new HashSet<string>();

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
		if (flags is { CleanIndex: true, Dry: false } && Directory.Exists(outputPath)) {
			Log.Information("Clearing existing install");
			Directory.Delete(outputPath, true);
		}

		foreach (var indexPath in indexFiles) {
			if (!File.Exists(indexPath)) {
				Log.Error("Index file {Path} does not exist", indexPath);
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

			using var httpHandler = new HttpClientHandler();
			httpHandler.CheckCertificateRevocationList = true;
			httpHandler.AllowAutoRedirect = true;
			httpHandler.AutomaticDecompression = DecompressionMethods.All;

			using var httpClient = new HttpClient(httpHandler, true);
			httpClient.DefaultRequestHeaders.UserAgent.Clear();
			httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Jackdaw/0.0.1 (Module/ResCacheDownloader)");

			var md5Lookup = new Dictionary<string, string>();
			var current = 0;

			// pass 2: create directories
			Log.Information("Creating directories");
			foreach (var record in records) {
				cacheRepo.Add(record.ResourcePath);
				targetCache.Add(record.ResourcePath);

				var target = Path.Combine(outputPath, record.Path.AbsolutePath[1..]);
				if (flags.Dry) {
					continue;
				}

				target.EnsureDirectoryExists();
			}

			// pass 3: actualize files
			Log.Information("Downloading files");
			foreach (var record in records) {
				var resPath = record.ResourcePath;

				if (!flags.NoDeduplication) {
					if (!md5Lookup.TryGetValue(record.MD5, out resPath)) {
						resPath = record.ResourcePath;
						md5Lookup.Add(record.MD5, resPath);
					}
				}

				var host = RES_DOMAIN;
				if (record.Path.Scheme == "app") {
					host = APP_DOMAIN;
				}

				var resFilePath = Path.Combine(cacheRoot, resPath);

				if (!File.Exists(resFilePath)) {
					Log.Information("Downloading {Path}", resPath);
					if (flags is { Dry: false, NoDownload: false }) {
						resFilePath.EnsureDirectoryExists();
						await using var local = File.OpenWrite(resFilePath);
						await using var remote = await httpClient.GetStreamAsync(new Uri(host, resPath));
						await remote.CopyToAsync(local);
					}
				}

				Log.Information("[{Current}/{Total}/{Percent:F2}%] {Method} {Path}", ++current, records.Length, (float) current / records.Length * 100, method, record.Path.AbsolutePath[1..]);
				if (flags.Dry) {
					continue;
				}

				var target = Path.Combine(outputPath, record.Path.AbsolutePath[1..]);
				if (File.Exists(target)) {
					if (flags.NoOverwrite) {
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
		}

		if (flags.CleanIndex) {
			// pass 4: clearing cache files
			Log.Information("Reclaiming storage");
			foreach (var file in Directory.GetFiles(cacheRoot, "*", SearchOption.AllDirectories)) {
				var relative = Path.GetRelativePath(cacheRoot, file).Replace('\\', '/');
				if (relative.StartsWith('.') || relative.StartsWith("bundle", StringComparison.OrdinalIgnoreCase) || cacheRepo.Contains(relative)) {
					continue;
				}

				if (flags.Dry) {
					continue;
				}

				Log.Information("Deleting {File}", relative);
				File.Delete(file);
			}
		}

		WriteCacheList(cacheRoot, outputPath, targetCache);
	}

	private static void WriteCacheList(string cacheRoot, string outputPath, HashSet<string> cache) {
		var skipTarget = CRC.Create(CRC64Variants.Default).ComputeHashValue(Encoding.UTF8.GetBytes(outputPath)).ToString("x16");
		var indexPath = Path.Combine(cacheRoot, ".jackdaw");
		Directory.CreateDirectory(indexPath);
		using var stream = new FileStream(Path.Combine(indexPath, skipTarget + ".txt"), FileMode.Create, FileAccess.Write);
		using var writer = new StreamWriter(stream);
		foreach (var line in cache.Order()) {
			writer.WriteLine(line);
		}
	}

	private static HashSet<string> BuildCacheList(string cacheRoot, string outputPath) {
		var skipTarget = CRC.Create(CRC64Variants.Default).ComputeHashValue(Encoding.UTF8.GetBytes(outputPath)).ToString("x16");
		var indexPath = Path.Combine(cacheRoot, ".jackdaw");
		Directory.CreateDirectory(indexPath);

		var cache = new HashSet<string>();
		foreach (var txt in Directory.EnumerateFiles(indexPath, "*.txt", SearchOption.TopDirectoryOnly)) {
			if (Path.GetFileNameWithoutExtension(txt) == skipTarget) {
				continue;
			}

			using var stream = new FileStream(txt, FileMode.Open, FileAccess.Read);
			using var reader = new StreamReader(stream);
			while (reader.ReadLine() is { } line) {
				if (line.Trim().Length > 0) {
					cache.Add(line.Trim());
				}
			}
		}

		return cache;
	}
}
