/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveSpaceObjectDecal : ITr2Renderable, ITr2Pickable {
    public string? Name { get; set; }
    public bool Display { get; set; }
    [BlackArraySize(3)] public float[]? Position { get; set; }
    [BlackArraySize(4)] public float[]? Rotation { get; set; }
    [BlackArraySize(3)] public float[]? Scaling { get; set; }
    public int ParentBoneIndex { get; set; }
    public float MinScreenSize { get; set; }
    public Tr2Effect? DecalEffect { get; set; }
    public bool HasStaticIndexBuffers { get; set; }
}
