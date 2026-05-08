using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Jackdaw.Cache;
using Jackdaw.Structs.Client;
using Serilog;

namespace Jackdaw.RegistryHelper;

internal class Program {
	private const string START = "app:/start.ini";

	private static readonly ReadOnlyDictionary<string, string> DOWNLOAD_FILES = new Dictionary<string, string> {
		{ "app:/resfileindex.txt", "resource" },
		{ "app:/resfileindex_prefetch.txt", "prefetch" },
		{ "app:/resfileindex_Windows.txt", "windows" },
		{ "app:/EVE.app/Contents/Resources/build/resfileindex_macOS.txt", "mac" },
	}.AsReadOnly();

	private static readonly string[] DEFAULT_PLATFORMS = ["macOS"];

	private static JsonSerializerOptions JsonOptions { get; } = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	};

	private static async Task Main(string[] args) {
		if (args.Length < 1) {
			Console.WriteLine("Usage: Jackdaw.RegistryHelper <path/to/flycatcher> [versions...]");
			return;
		}

		Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Console().CreateLogger();

		var flycatcherRoot = args[0];

		var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) {
			NewLine = "\n",
		};

		await using var redirectStream = new FileStream(Path.Combine(flycatcherRoot, "redirect.registry"), FileMode.OpenOrCreate, FileAccess.ReadWrite);
		using var redirectReader = new StreamReader(redirectStream);
		Dictionary<string, string> redirectMap;
		using (var redirectCsvReader = new CsvReader(redirectReader, CultureInfo.InvariantCulture, true)) {
			#pragma warning disable CA1849
			redirectMap = redirectCsvReader.GetRecords<Redirection>().ToDictionary(x => x.From, x => x.To);
			#pragma warning restore CA1849
		}

		redirectStream.Seek(0, SeekOrigin.End);
		await using var redirectWriter = new StreamWriter(redirectStream);
		redirectWriter.NewLine = "\n";
		await using var redirectCsvWriter = new CsvWriter(redirectWriter, csvConfig);

		await using var versionStream = new FileStream(Path.Combine(flycatcherRoot, "version.registry"), FileMode.OpenOrCreate, FileAccess.ReadWrite);
		using var versionReader = new StreamReader(versionStream);
		HashSet<string> versionSet;
		using (var versionCsvReader = new CsvReader(versionReader, CultureInfo.InvariantCulture, true)) {
			#pragma warning disable CA1849
			versionSet = versionCsvReader.GetRecords<VersionRegistryRecord>().Select(x => x.Build).ToHashSet();
			#pragma warning restore CA1849
		}

		versionStream.Seek(0, SeekOrigin.End);
		await using var versionWriter = new StreamWriter(versionStream);
		versionWriter.NewLine = "\n";
		await using var versionCsvWriter = new CsvWriter(versionWriter, csvConfig);

		using var httpHandler = new HttpClientHandler();
		httpHandler.CheckCertificateRevocationList = true;
		httpHandler.AllowAutoRedirect = true;
		httpHandler.AutomaticDecompression = DecompressionMethods.All;

		using var httpClient = new HttpClient(httpHandler, true);
		httpClient.BaseAddress = new Uri("https://binaries.eveonline.com/", UriKind.Absolute);

		httpClient.DefaultRequestHeaders.UserAgent.Clear();
		httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Jackdaw/0.0.1 (Automated; Module/VersionChecker)");

		if (args.Length > 1) {
			if (args[1..] is ["all"]) {
				var directory = Path.Combine(flycatcherRoot, "index");
				foreach (var file in Directory.EnumerateFiles(directory, "eveonline_*.txt.zst", SearchOption.TopDirectoryOnly)) {
					var name = Path.GetFileName(file);
					var dotIndex = name.IndexOf('.', StringComparison.Ordinal);
					if (dotIndex == -1) {
						continue;
					}

					var baseName = name[..dotIndex];
					var versionIndex = baseName.IndexOf('_', StringComparison.Ordinal);
					if (versionIndex == -1) {
						continue;
					}

					var version = baseName[(versionIndex + 1)..];
					await ProcessVersion(versionSet, version, string.Empty, DEFAULT_PLATFORMS, httpClient, ShardProduct.EVE, ShardInfo.Fenris, flycatcherRoot, redirectMap, redirectCsvWriter, versionCsvWriter);
				}
			} else {
				foreach (var version in args[1..]) {
					var info = args[0].ToLower() switch {
						           "N" => ShardInfo.NetEase,
						           "F" => ShardInfo.FenrisFrontier,
						           "V" => ShardInfo.FenrisVanguard,
						           _ => ShardInfo.Fenris,
					           };

					await ProcessVersion(versionSet, version, info.RegionPrefix, DEFAULT_PLATFORMS, httpClient, ShardProduct.EVE, info, info.Region is ShardRegion.Fenris ? flycatcherRoot : Path.Combine(flycatcherRoot, info.Region.ToString().ToLower()), redirectMap, redirectCsvWriter, versionCsvWriter);
				}
			}
		} else {
			foreach (var server in Enum.GetValues<ShardServer>()) {
				if (server.IsDeprecated()) {
					continue;
				}

				foreach (var product in Enum.GetValues<ShardProduct>()) {
					try {
						if (!server.IsValidFor(product)) {
							continue;
						}

						var info = server.ToRegion() switch {
							           ShardRegion.NetEase => ShardInfo.NetEase,
							           ShardRegion.Frontier => ShardInfo.FenrisFrontier,
							           ShardRegion.Fenris when product is ShardProduct.Vanguard => ShardInfo.FenrisVanguard,
							           _ => ShardInfo.Fenris,
						           };

						Log.Information("Getting {Product} version for {Server}", product, server);
						var uri = new Uri(info.VerDomain, $"{product.ToProductName()}_{server.ToShortcode()}.json");
						Log.Information("Downloading {Uri}", uri);
						var json = await JsonSerializer.DeserializeAsync<ClientInfo>(await httpClient.GetStreamAsync(uri), JsonOptions, CancellationToken.None);
						if (json == null) {
							Log.Error("Failed to get {Product} version for {Server}", product, server);
							continue;
						}

						if (json.Protected) {
							Log.Error("Failed to get {Product} version for {Server} because it is protected", product, server);
							continue;
						}

						var version = json.Build;
						await ProcessVersion(versionSet, version, info.RegionPrefix, json.Platforms ?? DEFAULT_PLATFORMS, httpClient, product, info, server.ToRegion() is ShardRegion.Fenris ? flycatcherRoot : Path.Combine(flycatcherRoot, server.ToRegion().ToString().ToLower()), redirectMap, redirectCsvWriter, versionCsvWriter);
					} catch {
						Log.Error("Failed to get {Product} version for {Server}", product, server);
					}
				}
			}
		}
	}

	private static async Task ProcessVersion(HashSet<string> versionSet, string version, string prefix, string[] platforms, HttpClient httpClient, ShardProduct product, ShardInfo info, string root, Dictionary<string, string> redirect, CsvWriter redirectWriter, CsvWriter versionWriter) {
		Directory.CreateDirectory(root);

		version = version.Split(":")[^1];

		var combined = prefix + version;
		if (versionSet.Contains(combined)) {
			Log.Information("Version {Prefix}{Version} is already in the registry", prefix, version);
			return;
		}

		if (redirect.ContainsKey(combined)) {
			Log.Information("Version for {Prefix}{Version} is already in the registry", prefix, version);
			return;
		}

		await ProcessVersionCore(version, prefix, platforms, httpClient, product, info, root, redirect, redirectWriter, versionWriter);
	}

	private static async Task ProcessVersionCore(string version, string prefix, string[] platforms, HttpClient httpClient, ShardProduct product, ShardInfo info, string root, Dictionary<string, string> redirect, CsvWriter redirectWriter, CsvWriter versionWriter) {
		var versionInfo = new VersionRegistryRecord {
			Build = version,
			Platforms = string.Join(",", platforms),
		};

		await using var index = await Download(httpClient, new Uri(info.VerDomain, $"{product.ToClientName()}_{version}.txt"),
		                                       Path.Combine(root, "index"), $"{product.ToClientName()}_{version}.txt");
		if (await ProcessIndex(httpClient, info, index, root, prefix, versionInfo, true) == false) {
			Log.Error("Failed to process index for {Version}", version);
			return;
		}

		foreach (var platform in platforms) {
			if (string.IsNullOrEmpty(platform)) {
				continue;
			}

			await using var platformIndex = await Download(httpClient,
			                                               new Uri(info.VerDomain, $"{product.ToClientName()}{platform}_{version}.txt"), Path.Combine(root, "index"),
			                                               $"{product.ToClientName()}{platform}_{version}.txt");
			await ProcessIndex(httpClient, info, platformIndex, root, prefix, versionInfo, false);
		}

		if (string.IsNullOrEmpty(prefix) && versionInfo.Build != version && !redirect.ContainsKey(version)) {
			redirect.Add(version, versionInfo.Build);
			redirectWriter.WriteRecord(new Redirection {
				From = version,
				To = versionInfo.Build,
			});
			await redirectWriter.NextRecordAsync();
		}

		versionWriter.WriteRecord(versionInfo);
		await versionWriter.NextRecordAsync();
	}

	private static async Task<bool> ProcessIndex(HttpClient client, ShardInfo info, Stream indexStream, string root, string prefix, VersionRegistryRecord version, bool process) {
		var index = IndexParser.Parse(indexStream);
		if (process) {
			var start = index.FirstOrDefault(x => x.Path.OriginalString.Equals(START, StringComparison.OrdinalIgnoreCase));
			if (start == null) {
				Log.Error("Failed to find {Start} in index", START);
				return false;
			}

			await using var startStream = await Load(client, new Uri(info.AppDomain, start.ResourcePath), Path.Combine(root, "start"), $"start_{version.Build}.ini");
			if (!startStream.CanRead) {
				Log.Error("Failed to read {Start}", START);
				return false;
			}

			var startInfo = StartParser.Parse(startStream);
			if (string.IsNullOrEmpty(startInfo.Version)) {
				Log.Error("Failed to read {Start}", START);
				return false;
			}

			version.Build = prefix + startInfo.Build;
			version.Version = startInfo.Version;
			version.Codename = startInfo.Codename;
			version.Branch = startInfo.Branch;
			Log.Information("Found version {Version} for {Build}, Full = {Full}", version.Version, version.Build, version);
		}

		foreach (var file in index) {
			if (DOWNLOAD_FILES.TryGetValue(file.Path.OriginalString, out var type)) {
				var name = Path.GetFileNameWithoutExtension(file.Path.OriginalString) + $"_{version.Build}.txt";
				await (await Download(client, new Uri(info.AppDomain, file.ResourcePath), Path.Combine(root, type), name)).DisposeAsync();
			}
		}

		return true;
	}

	private static async Task<Stream> Download(HttpClient client, Uri uri, string folder, string name) {
		var fileName = Path.Combine(folder, name + ".zst");
		if (Path.Exists(fileName)) {
			Log.Information("{Uri} already downloaded", uri);
			await using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using var decompressed = JackdawUtils.Decompress(fileStream);
			var stream = new MemoryStream(decompressed.Length);
			stream.Write(decompressed.Span);
			stream.Seek(0, SeekOrigin.Begin);
			return stream;
		}

		Directory.CreateDirectory(folder);

		Log.Information("Downloading {Uri}", uri);
		try {
			var stream = new MemoryStream(await client.GetByteArrayAsync(uri)) {
				Position = 0,
			};
			using var compressed = JackdawUtils.Compress(stream);
			await using var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
			fileStream.Write(compressed.Span);
			stream.Position = 0;
			return stream;
		} catch {
			return Stream.Null;
		}
	}

	private static async Task<Stream> Load(HttpClient client, Uri uri, string folder, string name) {
		Directory.CreateDirectory(folder);

		Log.Information("Loading {Uri}", uri);
		try {
			var stream = new MemoryStream(await client.GetByteArrayAsync(uri)) {
				Position = 0,
			};
			var fileName = Path.Combine(folder, name);
			await using var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
			await stream.CopyToAsync(fileStream);
			stream.Position = 0;
			return stream;
		} catch {
			return Stream.Null;
		}
	}

	internal record Redirection {
		public string From { get; set; } = null!;
		public string To { get; set; } = null!;
	}
}
