using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jackdaw.Structs.Client;

public record ClientInfo {
	public string Build { get; set; } = null!;
	[JsonConverter(typeof(MixedBoolConverter))]
	public bool Protected { get; set; }
	public string[]? Platforms { get; set; } = [];
}

public class MixedBoolConverter : JsonConverter<bool> {
	public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		return reader.TokenType switch {
			       JsonTokenType.String => reader.GetString()?.Equals("true", StringComparison.OrdinalIgnoreCase) == true,
			       JsonTokenType.Number => reader.GetInt64() == 1,
			       JsonTokenType.True => true,
			       _ => false,
		       };
	}

	public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) {
		writer.WriteBooleanValue(value);
	}
}
