using System.Runtime.InteropServices;

namespace Knit.Meta;

[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0x8)]
public record struct Granny2SectorPointer {
	public int Offset { get; set; }
	public int Count { get; set; }
}
