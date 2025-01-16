using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.MemoryHandlers.Minidump;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public record struct MinidumpMemoryDescriptor64 {
	public nint StartOfMemoryRange { get; set; }
	public long Size { get; set; }
}
