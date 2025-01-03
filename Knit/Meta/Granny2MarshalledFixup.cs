using System.Runtime.InteropServices;

namespace Knit.Meta;

[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0x10)]
public record struct Granny2MarshalledFixup {
	public int Count { get; set; }
	public Granny2Fixup Fixup { get; set; }
}
