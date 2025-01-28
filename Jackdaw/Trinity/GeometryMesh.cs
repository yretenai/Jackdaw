using System.Buffers;
using System.IO;
using Jackdaw.Granny;
using Knit;
using Knit.glTF;
using Knit.TypeDefinitions;

namespace Jackdaw.Trinity;

public class GeometryMesh {
	public unsafe GeometryMesh(IMemoryOwner<byte> buffer) {
		var pin = buffer.Memory.Pin();
		using var stream = new UnmanagedMemoryStream((byte*) pin.Pointer, buffer.Memory.Length);
		using var granny = new Granny2File(stream);
		granny.TypeResolver = type => type == "MeshBoundsInfo" ? typeof(MeshBoundsInfo) : null;
		Resource = granny.LoadRoot()!;
	}

	public GrannyFileRoot Resource { get; }

	public void SaveAsGLTF(string path) {
		using var gltf = new GrannyGLTF(Resource);
		gltf.Write(path);
	}
}
