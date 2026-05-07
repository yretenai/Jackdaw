namespace Jackdaw.Blue;

public record BlueField {
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public BlueTypeId Type { get; set; }
	public long Offset { get; set; }
	public long Size { get; set; }
	public string? ClassType { get; set; }
	public BlueVarEditFlags EditFlags { get; set; }
	public List<BlueFieldChoice>? Choices { get; set; }
}

public record BlueFieldChoice {
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public object? Value { get; set; }
}

[Flags]
public enum BlueVarEditFlags : uint {
	None = 0,

	Read = 0x001,
	Write = 0x002,
	Notify = 0x004,
	Hidden = 0x008,

	Persist = 0x010,
	ReadOnly = 0x020,

	Flags = 0x100,
	Enum = 0x200,
}

