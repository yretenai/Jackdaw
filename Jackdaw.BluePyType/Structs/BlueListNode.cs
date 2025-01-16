using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public record struct BlueListNode {
	public BluePtr<BlueClass> Left { get; set; }
	public BluePtr<BlueClass> Right { get; set; }
	public BluePtr<BlueClass> Up { get; set; }
}
