using System.Text.Json.Serialization;

namespace Jackdaw.Structs.Trinity.Schema;

public record BlackSchemaCLSID {
	[JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
	[JsonPropertyName("namespace")] public string Namespace { get; set; } = string.Empty;
}
