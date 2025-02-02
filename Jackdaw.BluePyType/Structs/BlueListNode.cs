using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public record struct BlueListNode {
	public nint Left { get; set; }
	public nint Right { get; set; }
	public nint Up { get; set; }
}
