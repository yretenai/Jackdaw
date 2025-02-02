using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public record struct BluePyType {
	public nint Id { get; set; }
	public nint ClassId { get; set; }
	public nint Description { get; set; }
	public nint Interfaces { get; set; }
	public nint Fields { get; set; }
	public nint Unk1 { get; set; }
	public nint Parent { get; set; }
	public nint Unk2 { get; set; }
	public nint Unk3 { get; set; }
	public nint Unk4 { get; set; }
	public nint Unk5 { get; set; }
	public nint Unk6 { get; set; }
}
