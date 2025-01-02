using System.Runtime.InteropServices;

namespace Jackdaw.Structs.FSD;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct FSDSlice {
	public long Offset { get; set; }
	public long Length { get; set; }
}
