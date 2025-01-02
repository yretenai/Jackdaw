/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveTransform : Tr2Transform, IEveTransform, IEveSpaceObject2, ITr2Pickable, IWorldPosition {
    public bool HideOnLowQuality { get; set; }
    public float VisibilityThreshold { get; set; }
    public object[]? ParticleEmitters { get; set; }
    public object[]? ParticleSystems { get; set; }
    public object[]? Observers { get; set; }
    public bool UseLodLevel { get; set; }
    public int LodLevel { get; set; }
    public Tr2MeshBase? MeshLod { get; set; }
    public object[]? Children { get; set; }
    [BlackArraySize(3)] public float[]? OverrideBoundsMin { get; set; }
    [BlackArraySize(3)] public float[]? OverrideBoundsMax { get; set; }
}