using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using CommunityToolkit.HighPerformance.Buffers;
using DragonLib;
using Jackdaw.Exceptions;
using Jackdaw.Structs.Trinity;
using Serilog;

namespace Jackdaw.Trinity;

public class BlackFile {
	static BlackFile() {
		Types = typeof(BlackHeader).Assembly.GetTypes().Where(x => x.Namespace?.StartsWith("Jackdaw.Structs.Trinity.Generated", StringComparison.Ordinal) == true).ToDictionary(x => x.Name, x => x);
	}

	public BlackFile(MemoryOwner<byte> buffer) {
		var offset = 0;
		Header = MemoryMarshal.Read<BlackHeader>(buffer.Span);
		offset += Unsafe.SizeOf<BlackHeader>();

		var blob = MemoryMarshal.Read<BlackBlob>(buffer.Span[offset..]);
		offset += Unsafe.SizeOf<BlackBlob>();
		{
			var poolOffset = offset;
			StringPool = new string[blob.Count];
			for (var i = 0; i < StringPool.Length; i++) {
				StringPool[i] = buffer.Span[poolOffset..].ReadString(Encoding.UTF8) ?? string.Empty;
				poolOffset += Encoding.UTF8.GetBytes(StringPool[i]).Length + 1;
			}
		}
		offset += blob.Size - 2;

		blob = MemoryMarshal.Read<BlackBlob>(buffer.Span[offset..]);
		offset += Unsafe.SizeOf<BlackBlob>();
		{
			var poolOffset = offset;
			NamePool = new string[blob.Count];
			for (var i = 0; i < NamePool.Length; i++) {
				NamePool[i] = MemoryMarshal.Cast<byte, ushort>(buffer.Span[poolOffset..]).ReadString(Encoding.Unicode) ?? string.Empty;
				poolOffset += Encoding.Unicode.GetBytes(NamePool[i]).Length + 2;
			}
		}
		offset += blob.Size - 2;

		var span = buffer.Span[offset..];
		Root = ReadObject(ref span, true);
	}

	private static Dictionary<string, Type> Types { get; }

	public BlackHeader Header { get; set; }
	public string[] StringPool { get; set; }
	public string[] NamePool { get; set; }
	public Dictionary<uint, object> Objects { get; set; } = new();
	public object Root { get; set; }

	private object ReadObject(ref Span<byte> block, bool hasId) {
		var id = uint.MaxValue;
		if (hasId) {
			id = BinaryPrimitives.ReadUInt32LittleEndian(block);
			block = block[4..];
		}

		if (id == 0) {
			return null!;
		}

		if (id != uint.MaxValue && Objects.TryGetValue(id, out var obj)) {
			return obj;
		}

		var size = BinaryPrimitives.ReadInt32LittleEndian(block);
		block = block[4..];
		var type = StringPool[BinaryPrimitives.ReadUInt16LittleEndian(block)];
		block = block[2..];

		if (!Types.TryGetValue(type, out var t)) {
			throw new UnknownBlueObjectException(type);
		}

		var chunk = block[..(size - 2)];
		var properties = t.GetProperties().ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);
		block = block[(size - 2)..];

		obj = Activator.CreateInstance(t);
		if (obj == null) {
			throw new FailedBlueObjectCreationException(type);
		}

		while (chunk.Length > 0) {
			var name = StringPool[BinaryPrimitives.ReadUInt16LittleEndian(chunk)].Replace(" ", "", StringComparison.Ordinal);
			chunk = chunk[2..];
			if (!properties.TryGetValue(name, out var property)) {
				throw new UnknownBluePropertyException(name, type);
			}

			if (property.GetCustomAttribute<BlackExperimentalAttribute>() != null) {
				Log.Warning("Experimental property {Property} ({PropertyType}) is being read", name, property.PropertyType);
			}

			var value = ReadValue(ref chunk, property.PropertyType, property);
			property.SetValue(obj, value);
		}

		if (id != uint.MaxValue) {
			Objects[id] = obj;
		}

		return obj;
	}

	private object? ReadValue(ref Span<byte> chunk, Type type, MemberInfo member) {
		if (type.IsArray) {
			return ReadArray(ref chunk, type, member);
		}

		switch (type.FullName) {
			case "System.Boolean": {
				var value = chunk[0] != 0;
				chunk = chunk[1..];
				return value;
			}

			case "System.Byte": {
				var value = chunk[0];
				chunk = chunk[1..];
				return value;
			}

			case "System.SByte": {
				var value = (sbyte) chunk[0];
				chunk = chunk[1..];
				return value;
			}

			case "System.Int16": {
				var value = BinaryPrimitives.ReadInt16LittleEndian(chunk);
				chunk = chunk[2..];
				return value;
			}

			case "System.UInt16": {
				var value = BinaryPrimitives.ReadUInt16LittleEndian(chunk);
				chunk = chunk[2..];
				return value;
			}

			case "System.Int32": {
				var value = BinaryPrimitives.ReadInt32LittleEndian(chunk);
				chunk = chunk[4..];
				return value;
			}

			case "System.UInt32": {
				var value = BinaryPrimitives.ReadUInt32LittleEndian(chunk);
				chunk = chunk[4..];
				return value;
			}

			case "System.Int64": {
				var value = BinaryPrimitives.ReadInt64LittleEndian(chunk);
				chunk = chunk[8..];
				return value;
			}

			case "System.UInt64": {
				var value = BinaryPrimitives.ReadUInt64LittleEndian(chunk);
				chunk = chunk[8..];
				return value;
			}

			case "System.Single": {
				var value = BinaryPrimitives.ReadSingleLittleEndian(chunk);
				chunk = chunk[4..];
				return value;
			}

			case "System.Double": {
				var value = BinaryPrimitives.ReadDoubleLittleEndian(chunk);
				chunk = chunk[8..];
				return value;
			}

			case "System.String": {
				var pool = StringPool;
				if (member.GetCustomAttribute<BlackUseNamePoolAttribute>() != null) {
					pool = NamePool;
				}

				var value = pool[BinaryPrimitives.ReadUInt16LittleEndian(chunk)];
				chunk = chunk[2..];
				return value;
			}
		}

		if (type.IsClass || type.IsValueType || type.IsInterface) {
			return ReadObject(ref chunk, member.GetCustomAttribute<BlackPureRefAttribute>() == null);
		}

		throw new CatastrophicBlueException($"Unknown type {type.FullName}");
	}

	private object ReadArray(ref Span<byte> chunk, Type type, MemberInfo member) {
		ArgumentNullException.ThrowIfNull(member);

		var shouldReadSize = true;
		var size = 0;
		var attr = member.GetCustomAttribute<BlackArraySizeAttribute>();
		if (attr != null) {
			size = attr.Size;
			shouldReadSize = false;
		}

		var elementType = type.GetElementType();
		if (elementType == null) {
			throw new CatastrophicBlueException($"Failed to get element type for array {type.FullName}");
		}

		if (shouldReadSize) {
			size = BinaryPrimitives.ReadInt32LittleEndian(chunk);
			chunk = chunk[4..];
		}

		var pure = member.GetCustomAttribute<BlackPureRefAttribute>();
		if (pure is { Size: > 0 }) {
			size /= pure.Size;
		}

		if (member.GetCustomAttribute<BlackPureRefAttribute>() != null && type == typeof(byte[][])) {
			var elementSize = BinaryPrimitives.ReadInt16LittleEndian(chunk);
			chunk = chunk[2..];
			var array = new byte[size][];
			for (var i = 0; i < size; i++) {
				array[i] = chunk[..elementSize].ToArray();
				chunk = chunk[elementSize..];
			}

			return array;
		} else {
			var array = Array.CreateInstance(elementType, size);
			for (var i = 0; i < size; i++) {
				array.SetValue(ReadValue(ref chunk, elementType, member), i);
			}

			return array;
		}
	}
}
