using System.Text.Json.Serialization;

namespace Jackdaw.Structs.Trinity.Schema;

public record BlackSchemaType {
	[JsonPropertyName("interfaces")] public List<ulong> Interfaces { get; set; } = [];
	[JsonPropertyName("inherit")] public ulong Inherit { get; set; }
	[JsonPropertyName("description")] public string Description { get; set; } = string.Empty;
	[JsonPropertyName("address")] public ulong Address { get; set; }
	[JsonPropertyName("props")] public List<BlackSchemaProperty> Properties { get; set; } = [];
}
