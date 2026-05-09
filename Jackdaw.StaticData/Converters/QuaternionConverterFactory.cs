using System.Text.Json;
using System.Text.Json.Serialization;
using Silk.NET.Maths;

namespace Jackdaw.StaticData.Converters;

public class QuaternionConverterFactory : JsonConverterFactory {
	public override bool CanConvert(Type typeToConvert) => typeToConvert.IsConstructedGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Quaternion<>);
	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) => (JsonConverter) Activator.CreateInstance(typeof(Converter<>).MakeGenericType(typeToConvert.GetGenericArguments()[0]))!;

	public class Converter<T> : JsonConverter<Quaternion<T>> where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T> {
		public override Quaternion<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException("please implement me \ud83e\udd7a");

		public override void Write(Utf8JsonWriter writer, Quaternion<T> value, JsonSerializerOptions options) {
			writer.WriteStartObject();
			writer.WritePropertyName("X");
			JsonSerializer.Serialize(writer, value.X, options);
			writer.WritePropertyName("Y");
			JsonSerializer.Serialize(writer, value.Y, options);
			writer.WritePropertyName("Z");
			JsonSerializer.Serialize(writer, value.Z, options);
			writer.WritePropertyName("W");
			JsonSerializer.Serialize(writer, value.W, options);
			writer.WriteEndObject();
		}
	}
}
