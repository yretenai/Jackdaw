using System.Text.Json;
using System.Text.Json.Serialization;
using Jackdaw.Structs.FSD.Schema;

namespace Jackdaw.StaticData.Converters;

internal class FSDStringConverter : JsonConverter<FSDString> {
	public override FSDString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
	public override void Write(Utf8JsonWriter writer, FSDString value, JsonSerializerOptions options) => writer.WriteStringValue(value.Value);
}
