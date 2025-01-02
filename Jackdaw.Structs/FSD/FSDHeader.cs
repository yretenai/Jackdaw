using System.Runtime.InteropServices;

namespace Jackdaw.Structs.FSD;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct FSDHeader {
	public long Length { get; set; }
	public long HeaderSize { get; set; }
	public long EntryCount { get; set; }
	public long SliceCount { get; set; }
}
