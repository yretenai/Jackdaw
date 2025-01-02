namespace Jackdaw.Structs.Client;

public record VersionRegistryRecord {
	public string Build { get; set; } = null!;
	public string Version { get; set; } = null!;
	public string Codename { get; set; } = null!;
	public string Branch { get; set; } = null!;
	public string Platforms { get; set; } = string.Empty;
}
