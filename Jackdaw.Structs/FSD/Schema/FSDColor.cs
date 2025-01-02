using System.Runtime.InteropServices;

namespace Jackdaw.Structs.FSD.Schema;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x10)]
public record struct FSDColor {
	public float Red { get; set; }
	public float Green { get; set; }
	public float Blue { get; set; }
	public float Alpha { get; set; }
}
