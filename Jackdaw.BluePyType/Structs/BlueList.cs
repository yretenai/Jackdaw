using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.Structs;

[StructLayout(LayoutKind.Explicit, Pack = 8)]
public record struct BlueList {
	[field: FieldOffset(0x40)] public BluePtr<BlueListNode> Address { get; set; }
	[field: FieldOffset(0x48)] public long Count { get; set; }
	[field: FieldOffset(0x50)] public float Unknown { get; set; }
}
