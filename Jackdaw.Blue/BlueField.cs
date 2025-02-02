namespace Jackdaw.Blue;

public record BlueField {
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public long Type { get; set; }
	public long Offset { get; set; }
	public long Size { get; set; }
	public string ClassType { get; set; } = string.Empty;
	public ulong TypeId { get; set; }
}
