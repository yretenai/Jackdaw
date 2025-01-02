using Jackdaw.Structs.FSD.Schema;
using Newtonsoft.Json;

namespace Jackdaw.StaticData.Converters;

internal class FSDColorConverter : JsonConverter<FSDColor> {
	public override void WriteJson(JsonWriter writer, FSDColor value, JsonSerializer serializer) {
		writer.WriteStartArray();
		writer.WriteValue(value.Red);
		writer.WriteValue(value.Green);
		writer.WriteValue(value.Blue);
		writer.WriteValue(value.Alpha);
		writer.WriteEndArray();
	}

	public override FSDColor ReadJson(JsonReader reader, Type objectType, FSDColor existingValue, bool hasExistingValue, JsonSerializer serializer) => throw new NotImplementedException();
}
