using System.Runtime.InteropServices;

namespace Knit.Meta;

[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0xC)]
public record struct Granny2Fixup {
	public int FromOffset { get; set; }
	public Granny2Reference To { get; set; }
}
