using System.Text.Json.Serialization;

namespace Jackdaw.Structs.Trinity.Schema;

public record BlackSchemaRoot {
	[JsonPropertyName("id")] public Dictionary<ulong, string> IIDs { get; set; } = new();
	[JsonPropertyName("clsid")] public Dictionary<ulong, BlackSchemaCLSID> CLSIDs { get; set; } = new();
	[JsonPropertyName("types")] public Dictionary<ulong, BlackSchemaType> Types { get; set; } = new();
}
