using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.MemoryHandlers.Minidump;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public record struct MinidumpDirectory {
	public MinidumpStreamType Type { get; set; }
	public MinidumpLocationDescriptor Location { get; set; }
}
