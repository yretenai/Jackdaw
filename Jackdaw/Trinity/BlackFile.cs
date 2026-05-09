using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Jackdaw.Black;
using Jackdaw.Exceptions;
using Jackdaw.Structs.Trinity;
using JetBrains.Annotations;
using Pluto.IO.Binary;
using Serilog;

namespace Jackdaw.Trinity;

public class BlackFile {
	static BlackFile() {
		Types = typeof(IRoot).Assembly.GetTypes().ToDictionary(x => x.Name, x => x);
	}

	public BlackFile(BufferBinaryReader reader) {
		Header = reader.Read<BlackHeader>();

		var blob = reader.Read<BlackBlob>();
		var expected = reader.Position + blob.Size - 2;
		{
			StringPool = new string[blob.Count];
			for (var i = 0; i < StringPool.Length; i++) {
				StringPool[i] = reader.ReadCString<byte>(Encoding.UTF8);
			}
		}
		reader.Position = expected;

		blob = reader.Read<BlackBlob>();
		expected = reader.Position + blob.Size - 2;
		{
			WideStringPool = new string[blob.Count];
			for (var i = 0; i < WideStringPool.Length; i++) {
				WideStringPool[i] = reader.ReadCString<ushort>(Encoding.Unicode);
			}
		}
		reader.Position = expected;

		Root = ReadObject(reader, true);
	}

	private static Dictionary<string, Type> Types { get; }

	public BlackHeader Header { get; set; }
	public string[] StringPool { get; set; }
	public string[] WideStringPool { get; set; }
	public Dictionary<uint, object> Objects { get; set; } = new();
	public object Root { get; set; }

	private object ReadObject(BufferBinaryReader reader, bool hasId) {
		var id = uint.MaxValue;
		if (hasId) {
			id = reader.Read<uint>();
		}

		if (id == 0) {
			return null!;
		}

		if (id != uint.MaxValue && Objects.TryGetValue(id, out var obj)) {
			return obj;
		}

		var size = reader.Read<int>();
		if (size > reader.Length - reader.Position) {
			return null;
		}
		using var objectReader = new ArrayPoolBinaryReader(reader.ReadSharedBytes(size));
		var type = StringPool[objectReader.Read<ushort>()];

		if (!Types.TryGetValue(type, out var t)) {
			throw new UnknownBlueObjectException(type);
		}

		var properties = t.GetProperties().ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);

		obj = Activator.CreateInstance(t);
		if (obj == null) {
			throw new FailedBlueObjectCreationException(type);
		}

		while (objectReader.Length - objectReader.Position > 0) {
			var name = StringPool[objectReader.Read<ushort>()].Replace(" ", "", StringComparison.Ordinal);
			if (!properties.TryGetValue(name, out var property)) {
				throw new UnknownBluePropertyException(name, type);
			}

			if (property.GetCustomAttribute<BlackExperimentalAttribute>() != null) {
				Log.Warning("Experimental property {Property} ({PropertyType}) is being read", name, property.PropertyType);
			}

			var value = ReadValue(objectReader, property.PropertyType, property);
			property.SetValue(obj, value);
		}

		if (id != uint.MaxValue) {
			Objects[id] = obj;
		}

		return obj;
	}

	[UsedImplicitly] private static T ReadPrimitive<T>(BufferBinaryReader reader) where T : unmanaged => reader.Read<T>();

	private object? ReadValue(BufferBinaryReader reader, Type type, MemberInfo member) {
		if (type.IsArray) {
			return ReadArray(reader, type, member);
		}

		switch (type.IsConstructedGenericType) {
			case true when type.GetGenericTypeDefinition() == typeof(List<>):
				return ReadList(reader, type, member);
			case true when type.GetGenericTypeDefinition() == typeof(Dictionary<,>):
				return ReadDictionary(reader, type, member);
		}

		if (type.IsEnum) {
			var value = ReadValue(reader, type.GetEnumUnderlyingType(), member);
			return value == null ? Activator.CreateInstance(type) : Enum.ToObject(type, value);
		}

		if (type.IsPrimitive || type.IsValueType) {
			if (type == typeof(bool)) {
				return reader.Read<byte>() != 0;
			}

			return typeof(BlackFile).GetMethod("ReadPrimitive", BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(type).Invoke(null, [reader]);
		}

		if (type == typeof(string)) {
			var pool = StringPool;
			if (member.GetCustomAttribute<BlackUseNamePoolAttribute>() != null) {
				pool = WideStringPool;
			}

			return pool[reader.Read<ushort>()];
		}

		if (type.IsClass || type.IsValueType || type.IsInterface) {
			return ReadObject(reader, member.GetCustomAttribute<BlackArrayAttribute>() == null);
		}

		throw new CatastrophicBlueException($"Unknown type {type.FullName}");
	}

	private object? ReadList(BufferBinaryReader reader, Type type, MemberInfo member) {
		var elementType = type.GetGenericArguments()[0];
		if (elementType == null) {
			throw new CatastrophicBlueException($"Failed to get element type for array {type.FullName}");
		}

		var size = reader.Read<int>();
		var array = Activator.CreateInstance(type, size);
		var add = type.GetMethod("Add") ?? throw new UnreachableException();
		for (var i = 0; i < size; i++) {
			add.Invoke(array, [ReadValue(reader, elementType, member)]);
		}

		return array;
	}

	private object ReadDictionary(BufferBinaryReader reader, Type type, MemberInfo member) => throw new NotImplementedException();

	private object ReadArray(BufferBinaryReader reader, Type type, MemberInfo member) {
		var elementType = type.GetElementType();
		if (elementType == null) {
			throw new CatastrophicBlueException($"Failed to get element type for array {type.FullName}");
		}

		var size = reader.Read<int>();

		var pure = member.GetCustomAttribute<BlackArrayAttribute>();
		if (pure is { Size: > 0 }) {
			size /= pure.Size;
		}

		if (member.GetCustomAttribute<BlackArrayAttribute>() != null && type == typeof(byte[][])) {
			var elementSize = reader.Read<ushort>();
			var array = new byte[size][];

			for (var i = 0; i < size; i++) {
				array[i] = new byte[elementSize];
				reader.ReadBytes(array[i]);
			}

			return array;
		} else {
			var array = Array.CreateInstance(elementType, size);
			for (var i = 0; i < size; i++) {
				array.SetValue(ReadValue(reader, elementType, member), i);
			}

			return array;
		}
	}
}
