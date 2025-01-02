namespace Jackdaw.Structs.FSD.Schema;

[FSDStruct(0x9F8429EAE014BED4UL, 0x66419F7BB10C7711UL, FSDStructType.Dictionary)]
public class SOFMaterialSet : IFSDValue<SOFMaterialSet>, IFSDDict {
	public SOFMaterialSet(IFSDReader reader) {
		Key = reader.Read<ulong>();
		CustomMaterial1 = reader.ReadString();
		CustomMaterial2 = reader.ReadString();
		Description = reader.ReadString();
		Material1 = reader.ReadString();
		Material2 = reader.ReadString();
		Material3 = reader.ReadString();
		Material4 = reader.ReadString();
		ResPathInsert = reader.ReadString();
		SOFFactionName = reader.ReadString();
		SOFPatternName = reader.ReadString();
		SOFRaceHint = reader.ReadString();
		ColorHull = reader.Read<FSDColor>();
		ColorPrimary = reader.Read<FSDColor>();
		ColorSecondary = reader.Read<FSDColor>();
		ColorWindow = reader.Read<FSDColor>();
		// var bits = reader.Read<ulong>();
		reader.Offset += 8;
	}

	public string CustomMaterial1 { get; set; } // 0x10
	public string CustomMaterial2 { get; set; } // 0x20
	public string Description { get; set; } // 0x40
	public string Material1 { get; set; } // always
	public string Material2 { get; set; } // 0x100
	public string Material3 { get; set; } // 0x200
	public string Material4 { get; set; } // 0x400
	public string ResPathInsert { get; set; } // 0x800
	public string SOFFactionName { get; set; } // 0x1000
	public string SOFPatternName { get; set; } // 0x2000
	public string SOFRaceHint { get; set; } // 0x4000
	public FSDColor ColorHull { get; set; } // 0x1
	public FSDColor ColorPrimary { get; set; } // 0x2
	public FSDColor ColorSecondary { get; set; } // 0x4
	public FSDColor ColorWindow { get; set; } // 0x8

	public object Key { get; set; }

	public static SOFMaterialSet Read(IFSDReader reader) => new(reader);
}
