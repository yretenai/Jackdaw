using System.Text.Json.Serialization;

namespace Jackdaw.Structs.Trinity.Schema;

public record BlackSchemaProperty {
	[JsonPropertyName("address")] public ulong Address { get; set; }
	[JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
	[JsonPropertyName("description")] public string Description { get; set; } = string.Empty;
	[JsonPropertyName("type")] public BlackSchemaPropertyType Type { get; set; }
	[JsonPropertyName("offset")] public int Offset { get; set; }
	[JsonPropertyName("size")] public int Size { get; set; }
	[JsonPropertyName("iid")] public string IID { get; set; } = string.Empty;
}
