namespace Jackdaw.Structs.Client;

public enum ShardServer {
	// fenris
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
	Adam,

	// cn
	Serenity,
	Dragon,
	Unicorn,
	Aurora,
	Infinity,
	Hurricane,
	Storm,

	// frontier
	Stillness,
}

public enum ShardRegion {
	Fenris,
	Frontier,
	Vanguard,
	NetEase,
}

public enum ShardProduct {
	EVE,
	Frontier,
	Vanguard,
}

public static class ShardServerHelpers {
	public static ShardRegion ToRegion(this ShardServer server) =>
		server switch {
			ShardServer.Serenity => ShardRegion.NetEase,
			ShardServer.Dragon => ShardRegion.NetEase,
			ShardServer.Unicorn => ShardRegion.NetEase,
			ShardServer.Aurora => ShardRegion.NetEase,
			ShardServer.Infinity => ShardRegion.NetEase,
			ShardServer.Hurricane => ShardRegion.NetEase,
			ShardServer.Storm => ShardRegion.NetEase,
			ShardServer.Stillness => ShardRegion.Frontier,
			_ => ShardRegion.Fenris,
		};

	public static bool IsValidFor(this ShardServer server, ShardProduct product) {
		return product switch {
			       ShardProduct.Frontier => server is ShardServer.Stillness,
			       ShardProduct.Vanguard => server is ShardServer.Tranquility or ShardServer.Singularity or ShardServer.Thunderdome,
			       _ => server is not ShardServer.Stillness,
		       };
	}

	public static ShardProduct ToShardProduct(this ShardServer server) =>
		server switch {
			ShardServer.Stillness => ShardProduct.Frontier,
			_ => ShardProduct.EVE,
		};

	public static bool IsDeprecated(this ShardServer server) =>
		server switch {
			ShardServer.Tranquility => false,
			ShardServer.Singularity => false,
			ShardServer.Thunderdome => false,
			ShardServer.Chaos => false,
			ShardServer.Adam => false,
			ShardServer.Nebula => false,
			ShardServer.Serenity => false,
			ShardServer.Infinity => false,
			ShardServer.Stillness => false,
			_ => true,
		};

	public static string ToShortcode(this ShardServer server) =>
		server switch {
			ShardServer.Tranquility => "TQ",
			ShardServer.Singularity => "SISI",
			ShardServer.Multiplicity => "MP",
			_ => server.ToString().ToUpperInvariant(),
		};

	public static ShardServer ToShardServer(this string code) =>
		code.ToUpper() switch {
			"TQ" => ShardServer.Tranquility,
			"SISI" => ShardServer.Singularity,
			"MP" => ShardServer.Multiplicity,
			_ => Enum.Parse<ShardServer>(code, true),
		};

	public static string ToProductName(this ShardProduct product) =>
		product switch {
			ShardProduct.Vanguard => "evevanguard",
			_ => "eveclient",
		};

	public static string ToClientName(this ShardProduct product) =>
		product switch {
			ShardProduct.Vanguard => "evevanguard",
			_ => "eveonline",
		};
}
