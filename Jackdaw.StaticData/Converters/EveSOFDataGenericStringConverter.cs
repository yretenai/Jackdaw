using System.Text.Json;
using System.Text.Json.Serialization;
using Jackdaw.Black;

namespace Jackdaw.StaticData.Converters;

internal class EveSOFDataGenericStringConverter : JsonConverter<EveSOFDataGenericString> {
	public override EveSOFDataGenericString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		var value = reader.GetString();
		if (value == null) {
			return null;
		}

		return new EveSOFDataGenericString {
			Str = value,
		};
	}

	public override void Write(Utf8JsonWriter writer, EveSOFDataGenericString value, JsonSerializerOptions options) {
		writer.WriteStringValue(value.Str);
	}
}
