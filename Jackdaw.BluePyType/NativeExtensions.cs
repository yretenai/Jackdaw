using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Pluto.IO.DataReader;

namespace Jackdaw.BluePyType;

public static class NativeExtensions {
	public static T Read<T>(this nint address, int index = 0) where T : struct {
		if (address == nint.Zero) {
			return default;
		}

		var size = Unsafe.SizeOf<T>();
		var addr = address + size * index;
		Span<byte> bytes = stackalloc byte[size];
		IMemoryHandler.Handler.ReadBytes(addr, bytes, out _);
		return MemoryMarshal.Read<T>(bytes);
	}

	public static string ReadString(this nint address) {
		if (address == nint.Zero) {
			return string.Empty;
		}

		Span<byte> slop = stackalloc byte[0x200];
		if (!IMemoryHandler.Handler.ReadBytes(address, slop, out var read)) {
			return string.Empty;
		}

		var index = slop[..read].IndexOf((byte) 0);
		if (index == -1) {
			index = read;
		}

		return Encoding.UTF8.GetString(slop[..index]);
	}
}
