using System.Text.Json;
using System.Text.Json.Serialization;
using Jackdaw.Black;

namespace Jackdaw.StaticData.Converters;

internal class TriFloatConverter : JsonConverter<TriFloat> {
	public override TriFloat Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => new() {
		Value = (float) reader.GetDouble(),
	};

	public override void Write(Utf8JsonWriter writer, TriFloat value, JsonSerializerOptions options) => writer.WriteNumberValue(value.Value);
}
