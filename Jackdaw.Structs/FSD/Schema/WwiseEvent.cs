namespace Jackdaw.Structs.FSD.Schema;

[FSDStruct(0x8CA79B600F7DCB73UL, 0xED83B6A1C90F6C82, FSDStructType.Dictionary)]
public record struct WwiseEvent : IFSDValue<WwiseEvent>, IFSDDict {
	public WwiseEvent(IFSDReader reader) {
		Key = reader.ReadString();
		EventId = reader.Read<ulong>();
		EventName = reader.ReadString();
		StoppedByEvents = reader.ReadStringArray();
		PlaybackDuration = reader.ReadClass<WwisePlaybackEvent>();
		MaxRadiusAttenuation = reader.Read<float>();
		Is2D = reader.Read<bool>();
		IsLooping = reader.Read<bool>();
		IsVital = reader.Read<bool>();

		reader.Align();
	}

	public ulong EventId { get; set; }
	public string EventName { get; set; }
	public string[] StoppedByEvents { get; set; }
	public WwisePlaybackEvent PlaybackDuration { get; set; }
	public float MaxRadiusAttenuation { get; set; }
	public bool Is2D { get; set; }
	public bool IsLooping { get; set; }
	public bool IsVital { get; set; }

	public object Key { get; set; }

	public static WwiseEvent Read(IFSDReader reader) => new(reader);
}
