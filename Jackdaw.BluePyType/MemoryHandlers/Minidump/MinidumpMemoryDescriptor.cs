using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.MemoryHandlers.Minidump;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public record struct MinidumpMemoryDescriptor {
	public nint StartOfMemoryRange { get; set; }
	public MinidumpLocationDescriptor Memory { get; set; }
}
