using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance.Buffers;
using Jackdaw.Exceptions;
using Jackdaw.Structs.FSD;

namespace Jackdaw.FSD;

public class FSDBinary {
	private static readonly Dictionary<(ulong High, ulong Low), (Type Type, FSDStructType StructType)> Types = new();

	static FSDBinary() {
		var types = typeof(FSDStructAttribute).Assembly.GetTypes();
		foreach (var type in types) {
			var attribute = type.GetCustomAttribute<FSDStructAttribute>();
			if (attribute is not null) {
				Types.Add((attribute.High, attribute.Low), (type, attribute.Type));
			}
		}
	}

	public FSDBinary(MemoryOwner<byte> data) {
		IdHigh = MemoryMarshal.Read<ulong>(data.Memory.Span);
		IdLow = MemoryMarshal.Read<ulong>(data.Memory.Span[8..]);
		Hash = MemoryMarshal.Read<ulong>(data.Memory.Span[16..]);

		using var reader = new FSDReader(data.Slice(0x18, data.Length - 0x18));

		if (!Types.TryGetValue((IdHigh, IdLow), out var type)) {
			throw new UnknownStaticDataTypeException(IdHigh, IdLow);
		}

		Header = reader.Read<FSDHeader>();
		Value = new object[Header.EntryCount];

		var method = typeof(FSDBinary).GetMethod(nameof(ReadValue), BindingFlags.NonPublic | BindingFlags.Instance);
		var generic = method!.MakeGenericMethod(type.Type);
		Value = generic.Invoke(this, [reader, type.StructType])!;
	}

	public ulong IdHigh { get; }
	public ulong IdLow { get; }
	public ulong Hash { get; }
	public FSDHeader Header { get; }
	public object Value { get; }

	private object ReadValue<T>(IFSDReader reader, FSDStructType type) where T : IFSDValue<T> {
		switch (type) {
			case FSDStructType.Array: {
				var list = new List<T>((int) Header.EntryCount);
				for (var i = 0; i < Header.SliceCount; i++) {
					var array = reader.ReadClassArray<T>();
					list.AddRange(array);
				}

				return list;
			}
			case FSDStructType.Dictionary: {
				var dict = new Dictionary<object, T>((int) Header.EntryCount);
				for (var i = 0; i < Header.SliceCount; i++) {
					var array = reader.ReadClassArray<T>();
					foreach (var item in array) {
						if (item is not IFSDDict dictItem) {
							throw new MismatchedItemException("Item is not a dictionary item");
						}

						dict[dictItem.Key] = item;
					}
				}

				return dict;
			}
			case FSDStructType.Object:
			default:
				return reader.ReadClass<T>();
		}
	}
}
