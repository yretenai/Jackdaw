using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Jackdaw.BluePyType.MemoryHandlers;

namespace Jackdaw.BluePyType.Structs;

public readonly record struct BluePtr<T>(nint Address) where T : struct {
	public bool IsZero => Address == 0;

	public T Read(int index = 0) {
		if (IsZero) {
			return default;
		}

		var size = Unsafe.SizeOf<T>();
		var addr = Address + size * index;
		Span<byte> bytes = stackalloc byte[size];
		IMemoryHandler.Handler.ReadBytes(addr, bytes, out _);
		return MemoryMarshal.Read<T>(bytes);
	}

	public BluePtr<TOther> As<TOther>() where TOther : struct => new(Address);

	public string ReadString() {
		if (IsZero) {
			return string.Empty;
		}

		Span<byte> slop = stackalloc byte[0x200];
		if (!IMemoryHandler.Handler.ReadBytes(Address, slop, out var read)) {
			return string.Empty;
		}

		var index = slop[..read].IndexOf((byte) 0);
		if (index == -1) {
			Debugger.Break();
			index = read;
		}

		return Encoding.UTF8.GetString(slop[..index]);
	}
}
