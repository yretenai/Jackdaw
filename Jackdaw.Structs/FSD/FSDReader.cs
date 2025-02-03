using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Jackdaw.Structs.FSD;

public class FSDReader(Memory<byte> data) {
	public Memory<byte> Data { get; } = data;

	public int Offset { get; set; }

	public T Read<T>() where T : struct {
		var size = Unsafe.SizeOf<T>();
		var value = MemoryMarshal.Read<T>(Data.Span[Offset..]);
		Offset += size;
		return value;
	}

	public T[] ReadArray<T>() where T : struct {
		var offset = (int) Read<long>();
		var tmp = Offset;
		Offset = offset;
		var count = Read<long>();
		var array = count == 0 ? Array.Empty<T>() : new T[count];
		for (var i = 0; i < count; i++) {
			array[i] = Read<T>();
		}

		Offset = tmp;
		return array;
	}

	public string ReadString() {
		var offset = (int) Read<long>();
		var tmp = Offset;
		Offset = offset;
		var length = (int) Read<long>();
		var value = length == 0 ? string.Empty : Encoding.UTF8.GetString(Data.Span.Slice(Offset, length));
		Offset = tmp;
		return value;
	}

	public string[] ReadStringArray() {
		var offset = (int) Read<long>();
		var tmp = Offset;
		Offset = offset;
		var count = Read<long>();
		var array = count == 0 ? Array.Empty<string>() : new string[count];
		for (var i = 0; i < count; i++) {
			array[i] = ReadString();
		}

		Offset = tmp;
		return array;
	}

	public T ReadClass<T>() where T : IFSDValue<T> => T.Read(this);

	public T[] ReadClassArray<T>() where T : IFSDValue<T> {
		var offset = (int) Read<long>();
		var tmp = Offset;
		Offset = offset;
		var count = Read<long>();
		var array = count == 0 ? Array.Empty<T>() : new T[count];
		for (var i = 0; i < count; i++) {
			array[i] = T.Read(this);
		}

		Offset = tmp;
		return array;
	}

	public Dictionary<object, T> ReadClassDict<T>() where T : IFSDValue<T>, IFSDDict {
		var offset = (int) Read<long>();
		var entryCount = (int) Read<long>();
		var tmp = Offset;
		Offset = offset;
		var sliceCount = (int) Read<long>();
		var dict = new Dictionary<object, T>(entryCount);
		for (var i = 0; i < sliceCount; i++) {
			var sliceOffset = (int) Read<long>();
			var sliceTmp = Offset;
			Offset = sliceOffset;
			var sliceEntryCount = (int) Read<long>();
			for (var j = 0; j < sliceEntryCount; j++) {
				var value = ReadClass<T>();
				dict[value.Key] = value;
			}

			Offset = sliceTmp;
		}

		Offset = tmp;
		return dict;
	}

	public void Align() {
		Offset = unchecked(Offset + 3) & 0x7FFFFFFC;
	}
}

public interface IFSDValue<out T> {
	public static abstract T Read(FSDReader reader);
}

public interface IFSDDict {
	public object Key { get; }
}
