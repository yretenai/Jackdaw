using Jackdaw.Structs.FSD.Schema;
using Newtonsoft.Json;

namespace Jackdaw.StaticData.Converters;

public class FSDResourceConverter : JsonConverter<FSDResource> {
	public override void WriteJson(JsonWriter writer, FSDResource? value, JsonSerializer serializer) {
		writer.WriteValue(value!.Path);
	}

	public override FSDResource ReadJson(JsonReader reader, Type objectType, FSDResource? existingValue, bool hasExistingValue, JsonSerializer serializer) => throw new NotImplementedException();
}
