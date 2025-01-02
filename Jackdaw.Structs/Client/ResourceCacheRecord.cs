namespace Jackdaw.Structs.Client;

public record ResourceCacheRecord {
	public Uri Path { get; set; } = null!;
	public string ResourcePath { get; set; } = null!;
	public string MD5 { get; set; } = null!;
	public int Size { get; set; }

	public int CompressedSize { get; set; }
	// public int Permissions { get; set; }
}
