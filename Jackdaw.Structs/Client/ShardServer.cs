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
}

public enum ShardRegion {
	CCP,
	China,
}

public static class ShardServerHelpers {
	public static ShardRegion ToRegion(this ShardServer server) =>
		server switch {
			ShardServer.Serenity => ShardRegion.China,
			ShardServer.Dragon => ShardRegion.China,
			ShardServer.Unicorn => ShardRegion.China,
			ShardServer.Aurora => ShardRegion.China,
			_ => ShardRegion.CCP,
		};

	public static bool IsInternal(this ShardServer server) =>
		server switch {
			ShardServer.Tranquility => false,
			ShardServer.Singularity => false,
			ShardServer.Thunderdome => false,
			ShardServer.Serenity => false,
			ShardServer.Aurora => false,
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
