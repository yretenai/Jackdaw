namespace Jackdaw.Structs.FSD.Schema;

[FSDStruct(0xFBDAC5A9A509E464UL, 0xE518F3EF7E283764UL, FSDStructType.Dictionary)]
public class GraphicID : IFSDValue<GraphicID>, IFSDDict {
	private GraphicID(IFSDReader reader) {
		Key = reader.Read<ulong>();
		AnimationStateObjects = reader.ReadClassDict<FSDString>();
		ControllerVarriableOverrides = reader.ReadClassDict<ControllerVariableOverride>();
		GraphicFile = reader.ReadString();
		IconInfo = reader.ReadClass<FSDResource>();
		SOFFactionName = reader.ReadString();
		SOFHullName = reader.ReadString();
		SOFLayout = reader.ReadString();
		SOFRaceName = reader.ReadString();
		AlbedoColor = reader.Read<FSDColor>();
		AmmoColor = reader.Read<FSDColor>();
		EmissiveColor = reader.Read<FSDColor>();
		ExplosionBucketId = reader.Read<uint>();
		GraphicLocationId = reader.Read<uint>();
		SOFMaterialSetId = reader.Read<uint>();
		// var bits = reader.Read<uint>();
		reader.Offset += 4; // for some reason this is not 64-bits?!?!?!?
	}

	public Dictionary<object, FSDString> AnimationStateObjects { get; set; }
	public Dictionary<object, ControllerVariableOverride> ControllerVarriableOverrides { get; set; }
	public string GraphicFile { get; set; }
	public FSDResource IconInfo { get; set; }
	public string SOFFactionName { get; set; }
	public string SOFHullName { get; set; }
	public string SOFLayout { get; set; }
	public string SOFRaceName { get; set; }
	public FSDColor AlbedoColor { get; set; }
	public FSDColor AmmoColor { get; set; }
	public FSDColor EmissiveColor { get; set; }
	public uint ExplosionBucketId { get; set; }
	public uint GraphicLocationId { get; set; }
	public uint SOFMaterialSetId { get; set; }

	public object Key { get; set; }

	public static GraphicID Read(IFSDReader reader) => new(reader);
}
