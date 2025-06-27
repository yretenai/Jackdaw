namespace Jackdaw.Structs.Client;

public enum ShardServer {
	Tranquility,
	Singularity,
	Multiplicity,
	Thunderdome,
	Duality,
	Buckingham,
	Chaos,
	Nebula,
	Nova,
	Polaris,
	Roundhouse,
	Serenity,
	Dragon,
	Unicorn,
	Aurora,
	Infinity,
}

public enum ShardRegion {
	CCP,
	NetEase,
}

public static class ShardServerHelpers {
	public static ShardRegion ToRegion(this ShardServer server) =>
		server switch {
			ShardServer.Serenity => ShardRegion.NetEase,
			ShardServer.Dragon => ShardRegion.NetEase,
			ShardServer.Unicorn => ShardRegion.NetEase,
			ShardServer.Aurora => ShardRegion.NetEase,
			ShardServer.Infinity => ShardRegion.NetEase,
			_ => ShardRegion.CCP,
		};

	public static bool IsInternal(this ShardServer server) =>
		server switch {
			ShardServer.Tranquility => false,
			ShardServer.Singularity => false,
			ShardServer.Thunderdome => false,
			ShardServer.Serenity => false,
			ShardServer.Infinity => false,
			_ => true,
		};

	public static string ToShortcode(this ShardServer server) =>
		server switch {
			ShardServer.Tranquility => "TQ",
			ShardServer.Singularity => "SISI",
			ShardServer.Multiplicity => "MP",
			_ => server.ToString().ToUpperInvariant(),
		};

	public static ShardServer FromShortcode(string code) =>
		code.ToUpper() switch {
			"TQ" => ShardServer.Tranquility,
			"SISI" => ShardServer.Singularity,
			"MP" => ShardServer.Multiplicity,
			_ => Enum.Parse<ShardServer>(code, true),
		};
}
