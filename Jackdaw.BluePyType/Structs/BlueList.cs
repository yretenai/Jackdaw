using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.Structs;

[StructLayout(LayoutKind.Explicit, Pack = 8)]
public record struct BlueList {
	[field: FieldOffset(0x40)] public nint Address { get; set; }
	[field: FieldOffset(0x48)] public long Count { get; set; }
	[field: FieldOffset(0x50)] public float MaxTimeForPendingDeletes { get; set; }
	[field: FieldOffset(0x54)] public int MaxPendingDeletes { get; set; }
}
