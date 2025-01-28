using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Jackdaw.Granny;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public record struct Bounds {
	public Vector3 Min { get; set; }
	public Vector3 Max { get; set; }
}

public class MeshBoundsArea {
	public Bounds Bounds { get; set; }
	public int VertexCount { get; set; }
}

public class MeshBoundsInfo {
	public Bounds Bounds { get; set; }
	public List<MeshBoundsArea> AreaInfo { get; set; } = [];
	public int SourceMeshIndex { get; set; }
	public int MaxScreenSize { get; set; }
	public float[] UVDensities { get; set; } = [];
}
