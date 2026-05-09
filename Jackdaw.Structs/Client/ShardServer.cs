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

	// vanguard
	Live,
	VIP,
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
	extension(ShardServer server) {
		public ShardRegion Region =>
			server switch {
				ShardServer.Serenity => ShardRegion.NetEase,
				ShardServer.Dragon => ShardRegion.NetEase,
				ShardServer.Unicorn => ShardRegion.NetEase,
				ShardServer.Aurora => ShardRegion.NetEase,
				ShardServer.Infinity => ShardRegion.NetEase,
				ShardServer.Hurricane => ShardRegion.NetEase,
				ShardServer.Storm => ShardRegion.NetEase,
				ShardServer.Stillness => ShardRegion.Frontier,
				ShardServer.Live => ShardRegion.Vanguard,
				ShardServer.VIP => ShardRegion.Vanguard,
				_ => ShardRegion.Fenris,
			};

		public ShardProduct ShardProduct =>
			server switch {
				ShardServer.Stillness => ShardProduct.Frontier,
				ShardServer.Live or ShardServer.VIP => ShardProduct.Frontier,
				_ => ShardProduct.EVE,
			};

		public bool IsDeprecated =>
			server switch {
				ShardServer.Tranquility => false,
				ShardServer.Singularity => false,
				ShardServer.Thunderdome => false,
				ShardServer.Chaos => false,
				ShardServer.Nebula => false,
				ShardServer.Serenity => false,
				ShardServer.Infinity => false,
				ShardServer.Stillness => false,
				_ => true,
			};

		public string Short =>
			server switch {
				ShardServer.Tranquility => "TQ",
				ShardServer.Singularity => "SISI",
				ShardServer.Multiplicity => "MP",
				_ => server.ToString().ToUpperInvariant(),
			};

		public bool IsValidFor(ShardProduct product) =>
			product switch {
				ShardProduct.Frontier => server is ShardServer.Stillness,
				ShardProduct.Vanguard => server is ShardServer.Live or ShardServer.VIP,
				_ => server is not ShardServer.Stillness,
			};
	}

	extension(string str) {
		public ShardServer ShardServer =>
			str.ToUpper() switch {
				"TQ" => ShardServer.Tranquility,
				"SISI" => ShardServer.Singularity,
				"MP" => ShardServer.Multiplicity,
				_ => Enum.Parse<ShardServer>(str, true),
			};
	}

	extension(ShardProduct product) {
		public string ProductName =>
			product switch {
				ShardProduct.Vanguard => "evevanguard",
				_ => "eveclient",
			};

		public string ClientName =>
			product switch {
				ShardProduct.Vanguard => "evevanguard",
				_ => "eveonline",
			};
	}

	extension(ShardRegion region) {
		public ShardInfo Info =>
			region switch {
				ShardRegion.Fenris => ShardInfo.Fenris,
				ShardRegion.Frontier => ShardInfo.FenrisFrontier,
				ShardRegion.Vanguard => ShardInfo.FenrisVanguard,
				ShardRegion.NetEase => ShardInfo.NetEase,
				_ => throw new ArgumentOutOfRangeException(nameof(region), region, null)
			};
	}
}
