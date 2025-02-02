using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public record struct BlueClass {
	public BlueListNode Node { get; set; }
	public uint Unknown1 { get; set; }
	public uint Unknown2 { get; set; }
	public nint Id { get; set; }
	public nint PyType { get; set; }
	public ulong Unknown3 { get; set; }
	public uint Hash { get; set; }
}
