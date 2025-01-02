/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveChildMesh : EveEntity, IEveSpaceObjectDecalOwner, IEveSpaceObjectChild, ITr2Renderable, ITr2GrannyAnimationOwner, IEveSpaceObjectAttachmentOwner, ITr2LightOwner {
    public Tr2GrannyAnimation? AnimationUpdater { get; set; }
    public int LowestLodVisible { get; set; }
    public float MinScreenSize { get; set; }
    public float CurrentScreenSize { get; set; }
    public bool UseSRT { get; set; }
    public bool StaticTransform { get; set; }
    public float SortValueScale { get; set; }
    public float SortValueOffset { get; set; }
    public int Origin { get; set; }
    public object[]? Decals { get; set; }
    public object[]? Attachments { get; set; }
    public object[]? Lights { get; set; }
    public int ReflectionMode { get; set; }
    public string? Name { get; set; }
    public bool Display { get; set; }
    public bool UseSpaceObjectData { get; set; }
    public Tr2MeshBase? Mesh { get; set; }
    [BlackArraySize(4)] public float[]? Rotation { get; set; }
    [BlackArraySize(3)] public float[]? Translation { get; set; }
    [BlackArraySize(3)] public float[]? Scaling { get; set; }
    [BlackArraySize(16)] public float[]? LocalTransform { get; set; }
    [BlackArraySize(16)] public float[]? WorldTransform { get; set; }
    public object[]? TransformModifiers { get; set; }
}