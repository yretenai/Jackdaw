namespace Jackdaw.Structs.Client;

public record BundleInfo {
	public string Name { get; set; } = null!;
	public string Checksum { get; set; } = null!;
	public int Size { get; set; }
}
