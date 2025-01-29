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
	Polaris
}

public static class ShardServerHelpers {
	public static string ToShortcode(this ShardServer server) =>
		server switch {
			ShardServer.Tranquility => "TQ",
			ShardServer.Singularity => "SISI",
			ShardServer.Multiplicity => "MP",
			ShardServer.Thunderdome => "THUNDERDOME",
			ShardServer.Duality => "DUALITY",
			ShardServer.Buckingham => "BUCKINGHAM",
			ShardServer.Chaos => "CHAOS",
			ShardServer.Nebula => "NEBULA",
			ShardServer.Nova => "NOVA",
			ShardServer.Polaris => "POLARIS",
			_ => throw new ArgumentOutOfRangeException(nameof(server), server, null),
		};

	public static ShardServer FromShortcode(string code) =>
		code.ToUpper() switch {
			"TQ" => ShardServer.Tranquility,
			"SISI" => ShardServer.Singularity,
			"MP" => ShardServer.Multiplicity,
			"THUNDERDOME" => ShardServer.Thunderdome,
			"DUALITY" => ShardServer.Duality,
			"BUCKINGHAM" => ShardServer.Buckingham,
			"CHAOS" => ShardServer.Chaos,
			"NEBULA" => ShardServer.Nebula,
			"NOVA" => ShardServer.Nova,
			"POLARIS" => ShardServer.Polaris,
			_ => throw new ArgumentOutOfRangeException(nameof(code), code, null),
		};
}
