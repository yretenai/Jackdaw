using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.MemoryHandlers.Minidump;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public record struct MinidumpLocationDescriptor {
	public int Size { get; set; }
	public int RVA { get; set; }
}
