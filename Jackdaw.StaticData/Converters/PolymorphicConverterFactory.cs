using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jackdaw.StaticData.Converters;

internal class PolymorphicConverterFactory : JsonConverterFactory {
	public override bool CanConvert(Type typeToConvert) => (typeToConvert.IsClass || typeToConvert.IsInterface) &&
	                                                       !typeToConvert.IsArray &&
	                                                       typeToConvert != typeof(object) &&
	                                                       typeToConvert != typeof(string) &&
	                                                       !typeToConvert.IsConstructedGenericType;

	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) => new Converter();

	internal class Converter : JsonConverter<object> {
		public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

		public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) {
			writer.WriteStartObject();

			var properties = value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (var property in properties) {
				if (property.GetCustomAttribute<JsonIgnoreAttribute>() != null) {
					continue;
				}

				var propertyValue = property.GetValue(value);
				writer.WritePropertyName(property.Name);
				JsonSerializer.Serialize(writer, propertyValue, options);
			}

			writer.WriteEndObject();
		}
	}
}
