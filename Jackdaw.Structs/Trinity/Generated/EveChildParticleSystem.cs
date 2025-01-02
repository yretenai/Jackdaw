/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveChildParticleSystem : EveEntity, IEveSpaceObjectChild, ITr2Renderable {
    public bool UseSRT { get; set; }
    public bool StaticTransform { get; set; }
    public int ReflectionMode { get; set; }
    public bool UseDynamicLod { get; set; }
    public int LodClampLow { get; set; }
    public float LodFactorLow { get; set; }
    public float LodFactorMedium { get; set; }
    public float LodSphereRadius { get; set; }
    public float MinScreenSize { get; set; }
    public float CurrentScreenSize { get; set; }
    public string? Name { get; set; }
    public bool Display { get; set; }
    public Tr2InstancedMesh? Mesh { get; set; }
    public object[]? ParticleEmitters { get; set; }
    public object[]? ParticleSystems { get; set; }
    [BlackArraySize(4)] public float[]? Rotation { get; set; }
    [BlackArraySize(3)] public float[]? Translation { get; set; }
    [BlackArraySize(3)] public float[]? Scaling { get; set; }
    [BlackArraySize(16)] public float[]? LocalTransform { get; set; }
    [BlackArraySize(16)] public float[]? WorldTransform { get; set; }
    public object[]? TransformModifiers { get; set; }
}