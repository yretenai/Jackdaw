namespace Jackdaw.Structs.FSD.Schema;

public record FSDResource : IFSDValue<FSDResource> {
	private FSDResource(IFSDReader reader) {
		Path = reader.ReadString();
		// var bits = reader.Read<ulong>();
		reader.Offset += 8;
	}

	public string Path { get; set; }

	public static FSDResource Read(IFSDReader reader) => new(reader);
}
