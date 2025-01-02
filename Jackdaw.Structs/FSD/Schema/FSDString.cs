namespace Jackdaw.Structs.FSD.Schema;

public record FSDString : IFSDValue<FSDString>, IFSDDict {
	private FSDString(IFSDReader reader) {
		Key = reader.ReadString();
		Value = reader.ReadString();
	}

	public string Value { get; set; }

	public object Key { get; set; }

	public static FSDString Read(IFSDReader reader) => new(reader);
}
