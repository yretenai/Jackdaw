using Jackdaw.Structs.Trinity.Generated;
using Newtonsoft.Json;

namespace Jackdaw.StaticData.Converters;

internal class EveSOFDataGenericStringConverter : JsonConverter<EveSOFDataGenericString> {
	public override EveSOFDataGenericString? ReadJson(JsonReader reader, Type objectType, EveSOFDataGenericString? existingValue, bool hasExistingValue, JsonSerializer serializer) {
		var value = reader.ReadAsString();
		if (value == null) {
			return null;
		}

		return new EveSOFDataGenericString {
			Str = value,
		};
	}

	public override void WriteJson(JsonWriter writer, EveSOFDataGenericString? value, JsonSerializer serializer) {
		writer.WriteValue(value?.Str);
	}
}
