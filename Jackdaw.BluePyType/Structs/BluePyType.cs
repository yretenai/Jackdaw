using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public record struct BluePyType {
	public BluePtr<BlueID> Id { get; set; }
	public BluePtr<BlueCLSID> ClassId { get; set; }
	public BluePtr<byte> Description { get; set; }
	public BluePtr<BlueCLSIDLink> Interfaces { get; set; }
	public BluePtr<BluePyField> Fields { get; set; }
	public nint Unk1 { get; set; }
	public BluePtr<BluePtr<BlueID>> Parent { get; set; }
	public nint Unk2 { get; set; }
	public nint Unk3 { get; set; }
	public nint Unk4 { get; set; }
	public nint Unk5 { get; set; }
	public nint Unk6 { get; set; }
}
