using System;
using Jackdaw.Structs.Client;

namespace Jackdaw.Cache;

public readonly record struct ShardInfo {
	public required Uri VerDomain { get; init; }
	public required Uri AppDomain { get; init; }
	public required Uri ResDomain { get; init; }
	public required string RegionPrefix { get; init; }
	public required ShardRegion Region { get; init; }

	public static ShardInfo NetEase { get; } = new() {
		VerDomain = new Uri("https://eve-china-version-files.oss-cn-hangzhou.aliyuncs.com/", UriKind.Absolute),
		AppDomain = new Uri("https://ma79.gdl.netease.com/eve/binaries/", UriKind.Absolute),
		ResDomain = new Uri("https://ma79.gdl.netease.com/eve/resources/", UriKind.Absolute),
		RegionPrefix = "NetEase-",
		Region = ShardRegion.NetEase,
	};

	public static ShardInfo Fenris { get; } = new() {
		VerDomain = new Uri("https://binaries.eveonline.com", UriKind.Absolute),
		AppDomain = new Uri("https://binaries.eveonline.com", UriKind.Absolute),
		ResDomain = new Uri("https://resources.eveonline.com", UriKind.Absolute),
		RegionPrefix = string.Empty,
		Region = ShardRegion.Fenris,
	};

	public static ShardInfo FenrisFrontier { get; } = new() {
		VerDomain = new Uri("https://binaries.shared.reitnorf.com", UriKind.Absolute),
		AppDomain = new Uri("https://binaries.shared.reitnorf.com", UriKind.Absolute),
		ResDomain = new Uri("https://resources.shared.reitnorf.com", UriKind.Absolute),
		RegionPrefix = "Frontier-",
		Region = ShardRegion.Frontier,
	};

	public static ShardInfo FenrisVanguard { get; } = new() {
		VerDomain = new Uri("https://cdn.evevanguardtech.com", UriKind.Absolute),
		AppDomain = new Uri("https://cdn.evevanguardtech.com", UriKind.Absolute),
		ResDomain = new Uri("https://cdn.evevanguardtech.com", UriKind.Absolute),
		RegionPrefix = "Vanguard-",
		Region = ShardRegion.Vanguard,
	};
}
