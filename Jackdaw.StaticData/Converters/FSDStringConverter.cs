using Jackdaw.Structs.FSD.Schema;
using Newtonsoft.Json;

namespace Jackdaw.StaticData.Converters;

internal class FSDStringConverter : JsonConverter<FSDString> {
	public override void WriteJson(JsonWriter writer, FSDString? value, JsonSerializer serializer) {
		writer.WriteValue(value!.Value);
	}

	public override FSDString ReadJson(JsonReader reader, Type objectType, FSDString? existingValue, bool hasExistingValue, JsonSerializer serializer) => throw new NotImplementedException();
}
