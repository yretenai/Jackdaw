using System.Text.Json;
using System.Text.Json.Serialization;
using Jackdaw.Structs.FSD.Schema;

namespace Jackdaw.StaticData.Converters;

internal class FSDColorConverter : JsonConverter<FSDColor> {
	public override FSDColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

	public override void Write(Utf8JsonWriter writer, FSDColor value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		writer.WriteNumberValue(value.Red);
		writer.WriteNumberValue(value.Green);
		writer.WriteNumberValue(value.Blue);
		writer.WriteNumberValue(value.Alpha);
		writer.WriteEndArray();
	}
}
