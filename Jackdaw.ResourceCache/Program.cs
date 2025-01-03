using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DragonLib;
using DragonLib.CommandLine;
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

		var cacheRoot = flags.ResCache;
		var outputPath = flags.Output;

		foreach (var indexPath in flags.IndexFiles) {
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

			// pass 1: create directories
			Log.Information("Creating directories");
			foreach (var record in records) {
				var target = Path.Combine(outputPath, record.Path.AbsolutePath[1..]);
				if (flags.Dry) {
					continue;
				}

				target.EnsureDirectoryExists();
			}

			var md5Lookup = new Dictionary<string, string>();
			var current = 0;
			// pass 2: actualize files
			foreach (var record in records) {
				if (!md5Lookup.TryGetValue(record.MD5, out var resPath)) {
					resPath = record.ResourcePath;
					md5Lookup.Add(record.MD5, resPath);
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
	}
}
