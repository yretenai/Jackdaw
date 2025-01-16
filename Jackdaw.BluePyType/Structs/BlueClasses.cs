using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public record struct BlueClasses {
	public nint VT { get; set; }
	public BlueList GenericThunks { get; set; }
	public BlueList Classes { get; set; }
	public BlueMap ClassesByName { get; set; }
}
