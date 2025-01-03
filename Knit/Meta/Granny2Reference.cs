using System.Runtime.InteropServices;

namespace Knit.Meta;

[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0x8)]
public record struct Granny2Reference {
	public Granny2SectorId Sector { get; set; }
	public int Offset { get; set; }
}
