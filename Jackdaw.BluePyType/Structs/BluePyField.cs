using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public record struct BluePyField {
	public BluePtr<byte> Name { get; set; }
	public long Type { get; set; }
	public long Offset { get; set; }
	public long Size { get; set; }
	public BluePtr<BlueCLSID> ClassType { get; set; }
	public BluePtr<byte> Description { get; set; }
	public ulong TypeId { get; set; }
	public nint Unk1 { get; set; }
	public nint Unk2 { get; set; }
	public nint Unk3 { get; set; }
}
