namespace Jackdaw.Structs.Client;

public record ClientInfo {
	public string Build { get; set; } = null!;
	public bool Protected { get; set; }
	public string[]? Platforms { get; set; } = [];
}
