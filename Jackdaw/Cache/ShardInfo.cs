using System;

namespace Jackdaw.Cache;

public readonly record struct ShardInfo {
	public required Uri VerDomain { get; init; }
	public required Uri AppDomain { get; init; }
	public required Uri ResDomain { get; init; }

	public static ShardInfo NetEase { get; } = new() {
		VerDomain = new Uri("https://eve-china-version-files.oss-cn-hangzhou.aliyuncs.com/", UriKind.Absolute),
		AppDomain = new Uri("https://ma79.gdl.netease.com/eve/binaries/", UriKind.Absolute),
		ResDomain = new Uri("https://ma79.gdl.netease.com/eve/resources/", UriKind.Absolute),
	};

	public static ShardInfo CCP { get; } = new() {
		VerDomain = new Uri("https://binaries.eveonline.com", UriKind.Absolute),
		AppDomain = new Uri("https://binaries.eveonline.com", UriKind.Absolute),
		ResDomain = new Uri("https://resources.eveonline.com", UriKind.Absolute),
	};
}
