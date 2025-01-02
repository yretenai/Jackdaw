using Jackdaw.Structs.Trinity.Generated;
using Newtonsoft.Json;

namespace Jackdaw.StaticData.Converters;

internal class TriFloatConverter : JsonConverter<TriFloat> {
	public override TriFloat? ReadJson(JsonReader reader, Type objectType, TriFloat? existingValue, bool hasExistingValue, JsonSerializer serializer) {
		var value = reader.ReadAsDouble();
		if (value == null) {
			return null;
		}

		return new TriFloat {
			Value = (float) value,
		};
	}

	public override void WriteJson(JsonWriter writer, TriFloat? value, JsonSerializer serializer) {
		writer.WriteValue(value?.Value);
	}
}
