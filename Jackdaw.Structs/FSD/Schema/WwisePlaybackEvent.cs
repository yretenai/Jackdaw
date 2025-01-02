namespace Jackdaw.Structs.FSD.Schema;

public record struct WwisePlaybackEvent : IFSDValue<WwisePlaybackEvent> {
	public WwisePlaybackEvent(IFSDReader reader) {
		Type = reader.ReadString();
		Max = reader.Read<float>();
		Min = reader.Read<float>();
		// var bits = reader.Read<ulong>();
		reader.Offset += 8;
	}

	public string? Type { get; set; }
	public float Max { get; set; }
	public float Min { get; set; }

	public static WwisePlaybackEvent Read(IFSDReader reader) => new(reader);
}
