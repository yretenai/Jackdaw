using System.Runtime.InteropServices;

namespace Knit.Meta;

[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0x48)]
public record struct Granny2FileInfo {
	public const uint MinimumSupportedVersion = 6;
	public const uint LatestVersion = 7;

	public uint Version { get; set; }
	public int FileSize { get; set; }
	public uint Checksum { get; set; }
	public Granny2SectorPointer Sectors { get; set; }
	public Granny2Reference RootTypeDefinition { get; set; }
	public Granny2Reference Root { get; set; }
	public uint Type { get; set; }
	public Granny2ExtraTags ExtraTags { get; set; }
	public uint StringChecksum { get; set; }

	public bool IsSupported => Version is >= MinimumSupportedVersion and <= LatestVersion;
}
