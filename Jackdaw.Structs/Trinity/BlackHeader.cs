using System.Runtime.InteropServices;

namespace Jackdaw.Structs.Trinity;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
public record struct BlackHeader {
	public uint Magic { get; set; }
	public uint Version { get; set; }
}
