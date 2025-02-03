namespace Jackdaw.Structs.FSD.Schema;

public record ControllerVariableOverride : IFSDValue<ControllerVariableOverride>, IFSDDict {
	private ControllerVariableOverride(FSDReader reader) {
		Key = reader.ReadString();
		Value = reader.Read<float>();
		IntValue = reader.Read<int>();
	}

	public float Value { get; set; }
	public int IntValue { get; set; }

	public object Key { get; set; }

	public static ControllerVariableOverride Read(FSDReader reader) => new(reader);
}
