using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
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

		var cacheRepo = BuildCacheList(cacheRoot, outputPath).Select(x => x.ResourcePath).ToHashSet();
		var targetCache = new List<ResourceCacheRecord>();

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

			Log.Information("Removing changed files");
			foreach (var record in sourceCache) {
				var target = Path.Combine(outputPath, record.Path.AbsolutePath[1..]);
				if (!File.Exists(target)) {
					continue;
				}

				var existingRecord = totalRecords.FirstOrDefault(record.Path.Equals);
				if (existingRecord == null) {
					Log.Information("Deleting {Path} as it is deleted", record.Path.AbsolutePath[1..]);
					if (!flags.Dry) {
						File.Delete(outputPath);
					}

					continue;
				}

				if (existingRecord.MD5 != record.MD5 || existingRecord.Size != record.Size || existingRecord.ResourcePath != record.ResourcePath) {
					Log.Information("Deleting {Path} as it has changed", record.Path.AbsolutePath[1..]);
					if (!flags.Dry) {
						File.Delete(outputPath);
					}
				}
			}
		}

		cacheRepo.UnionWith(totalRecords.Select(x => x.ResourcePath));

		using var httpHandler = new HttpClientHandler();
		httpHandler.CheckCertificateRevocationList = true;
		httpHandler.AllowAutoRedirect = true;
		httpHandler.AutomaticDecompression = DecompressionMethods.All;

		using var httpClient = new HttpClient(httpHandler, true);
		httpClient.DefaultRequestHeaders.UserAgent.Clear();
		httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Jackdaw/1.0.0 (Module/ResCacheDownloader)");

		var md5Lookup = new Dictionary<string, string>();
		var current = 0;

		// pass 2: create directories
		Log.Information("Creating directories");
		foreach (var record in totalRecords.DistinctBy(x => Path.GetDirectoryName(x.Path.AbsolutePath[1..]))) {
			targetCache.Add(record);

			var target = Path.Combine(outputPath, record.Path.AbsolutePath[1..]);
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

			Log.Information("[{Current}/{Total}/{Percent:F2}%] {Method} {Path}", ++current, totalRecords.Count, (float) current / totalRecords.Count * 100, method, record.Path.AbsolutePath[1..]);
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

		if (flags.Reclaim) {
			// pass 4: clearing cache files
			Log.Information("Reclaiming storage");
			foreach (var file in Directory.GetFiles(cacheRoot, "*", SearchOption.AllDirectories)) {
				var relative = Path.GetRelativePath(cacheRoot, file).Replace('\\', '/');
				if (relative.StartsWith('.') || relative.StartsWith("bundle", StringComparison.OrdinalIgnoreCase) || cacheRepo.Contains(relative)) {
					continue;
				}

				Log.Information("Deleting {File}", relative);
				if (flags.Dry) {
					continue;
				}

				File.Delete(file);
			}
		}

		WriteCacheList(cacheRoot, outputPath, targetCache);
	}

	private static void WriteCacheList(string cacheRoot, string outputPath, List<ResourceCacheRecord> cache) {
		var skipTarget = CRC.Create(CRC64Variants.Default).ComputeHashValue(Encoding.UTF8.GetBytes(outputPath)).ToString("x16");
		var indexPath = Path.Combine(cacheRoot, ".jackdaw");
		Directory.CreateDirectory(indexPath);
		using var stream = new FileStream(Path.Combine(indexPath, skipTarget + ".txt"), FileMode.Create, FileAccess.Write);
		using var writer = new StreamWriter(stream);
		writer.NewLine = "\n";
		writer.WriteLine("# version: 2");
		writer.WriteLine($"# path: {outputPath}");
		using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) {
			HasHeaderRecord = false,
			Comment = '#',
			NewLine = "\n",
			AllowComments = true,
		});

		foreach (var record in cache.OrderBy(x => x.ResourcePath)) {
			csv.WriteRecord(record);
		}
	}

	private static List<ResourceCacheRecord> BuildCacheList(string cacheRoot, string outputPath, bool onlySelf = false) {
		var skipTarget = CRC.Create(CRC64Variants.Default).ComputeHashValue(Encoding.UTF8.GetBytes(outputPath)).ToString("x16");
		var indexPath = Path.Combine(cacheRoot, ".jackdaw");
		Directory.CreateDirectory(indexPath);

		var cache = new List<ResourceCacheRecord>();
		foreach (var txt in Directory.EnumerateFiles(indexPath, "*.txt", SearchOption.TopDirectoryOnly)) {
			var isSelf = Path.GetFileNameWithoutExtension(txt) == skipTarget;

			switch (isSelf) {
				case true when !onlySelf:
				case false when onlySelf:
					continue;
			}

			using var stream = new FileStream(txt, FileMode.Open, FileAccess.Read);
			using var reader = new StreamReader(stream);

			var isNew = reader.ReadLine()?.StartsWith("# version: ") == true;
			stream.Position = 0;

			if (isNew) {
				using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) {
					HasHeaderRecord = false,
					Comment = '#',
					AllowComments = true,
				});
				cache.AddRange(csv.GetRecords<ResourceCacheRecord>());
			} else {
				while (reader.ReadLine() is { } line) {
					line = line.Trim();

					if (line.StartsWith('#')) {
						continue;
					}

					if (line.Length > 0) {
						cache.Add(new ResourceCacheRecord {
							ResourcePath = line
						});
					}
				}
			}
		}

		return cache;
	}
}
