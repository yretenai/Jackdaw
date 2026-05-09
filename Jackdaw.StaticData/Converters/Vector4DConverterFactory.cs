using System.Text.Json;
using System.Text.Json.Serialization;
using Silk.NET.Maths;

namespace Jackdaw.StaticData.Converters;

public class Vector4DConverterFactory : JsonConverterFactory {
	public override bool CanConvert(Type typeToConvert) => typeToConvert.IsConstructedGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Vector4D<>);
	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) => (JsonConverter) Activator.CreateInstance(typeof(Converter<>).MakeGenericType(typeToConvert.GetGenericArguments()[0]))!;

	public class Converter<T> : JsonConverter<Vector4D<T>> where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T> {
		public override Vector4D<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException("please implement me \ud83e\udd7a");

		public override void Write(Utf8JsonWriter writer, Vector4D<T> value, JsonSerializerOptions options) {
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
