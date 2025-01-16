using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.Structs;

[StructLayout(LayoutKind.Explicit, Pack = 8)]
public record struct BlueMap {
	[field: FieldOffset(0x40)] public nint Address { get; set; }
	[field: FieldOffset(0x48)] public long Count { get; set; }
	[field: FieldOffset(0x90)] public nint Start { get; set; }
	[field: FieldOffset(0x98)] public nint Last { get; set; }
	[field: FieldOffset(0xA0)] public nint End { get; set; }
	[field: FieldOffset(0xA8)] public long MapCount { get; set; }
	[field: FieldOffset(0xB0)] public long Capacity { get; set; }
}
