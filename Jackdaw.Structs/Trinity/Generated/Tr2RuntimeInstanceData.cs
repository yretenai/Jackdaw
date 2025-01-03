/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2RuntimeInstanceData : ITr2InstanceData, ITr2GenericEmitter, ITr2GpuBuffer {
    public int Count { get; set; }
    [BlackArraySize(3)] public float[]? AabbMin { get; set; }
    [BlackArraySize(3)] public float[]? AabbMax { get; set; }
    public string? Name { get; set; }
    public Tr2ParticleSystem? ParticleSystem { get; set; }
}
