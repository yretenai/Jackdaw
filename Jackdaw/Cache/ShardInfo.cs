using System;

namespace Jackdaw.Cache;

public readonly record struct ShardInfo {
	public required Uri VerDomain { get; init; }
	public required Uri AppDomain { get; init; }
	public required Uri ResDomain { get; init; }
	public required string RegionPrefix { get; init; }

	public static ShardInfo NetEase { get; } = new() {
		VerDomain = new Uri("https://eve-china-version-files.oss-cn-hangzhou.aliyuncs.com/", UriKind.Absolute),
		AppDomain = new Uri("https://ma79.gdl.netease.com/eve/binaries/", UriKind.Absolute),
		ResDomain = new Uri("https://ma79.gdl.netease.com/eve/resources/", UriKind.Absolute),
		RegionPrefix = "NetEase-",
	};

	public static ShardInfo CCP { get; } = new() {
		VerDomain = new Uri("https://binaries.eveonline.com", UriKind.Absolute),
		AppDomain = new Uri("https://binaries.eveonline.com", UriKind.Absolute),
		ResDomain = new Uri("https://resources.eveonline.com", UriKind.Absolute),
		RegionPrefix = string.Empty,
	};

	public static ShardInfo CCPFrontier { get; } = new() {
		VerDomain = new Uri("https://binaries.shared.reitnorf.com", UriKind.Absolute),
		AppDomain = new Uri("https://binaries.shared.reitnorf.com", UriKind.Absolute),
		ResDomain = new Uri("https://resources.shared.reitnorf.com", UriKind.Absolute),
		RegionPrefix = "Frontier-",
	};

	public static ShardInfo CCPVanguard { get; } = new() {
		VerDomain = new Uri("https://cdn.evevanguardtech.com", UriKind.Absolute),
		AppDomain = new Uri("https://cdn.evevanguardtech.com", UriKind.Absolute),
		ResDomain = new Uri("https://cdn.evevanguardtech.com", UriKind.Absolute),
		RegionPrefix = "Vanguard-",
	};
}
