using System.Text.Json;
using System.Text.Json.Serialization;
using Jackdaw.Structs.FSD.Schema;

namespace Jackdaw.StaticData.Converters;

public class FSDResourceConverter : JsonConverter<FSDResource> {
	public override FSDResource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
	public override void Write(Utf8JsonWriter writer, FSDResource value, JsonSerializerOptions options) => writer.WriteStringValue(value.Path);
}
