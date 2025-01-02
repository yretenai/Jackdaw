/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2ParticleSystem : ITr2InstanceData, ITr2GpuBuffer {
    public int AliveCount { get; set; }
    public ITr2GenericEmitter? EmitParticleOnDeathEmitter { get; set; }
    public ITr2GenericEmitter? EmitParticleDuringLifeEmitter { get; set; }
    public bool RequiresSorting { get; set; }
    public bool UpdateSimulation { get; set; }
    public bool ApplyForce { get; set; }
    public bool ApplyAging { get; set; }
    public bool IsGlobal { get; set; }
    public object[]? Forces { get; set; }
    public object[]? Constraints { get; set; }
    [BlackArraySize(3)] public float[]? AabbMin { get; set; }
    [BlackArraySize(3)] public float[]? AabbMax { get; set; }
    public int PeakAliveCount { get; set; }
    public bool UseSimTimeRebase { get; set; }
    public int MaxParticleCount { get; set; }
    public int OriginalMaxParticles { get; set; }
    public long GpuStride { get; set; }
    public string? Name { get; set; }
    public object[]? Elements { get; set; }
    public bool IsValid { get; set; }
}