using System.Runtime.InteropServices;

namespace Jackdaw.Structs.Trinity;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 6)]
public record struct BlackBlob {
	public int Size { get; set; }
	public ushort Count { get; set; }
}
