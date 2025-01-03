/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class AudGameObjResource {
    public long ID { get; set; }
    public string? Name { get; set; }
    public object[]? Parameters { get; set; }
    [BlackUseNamePool] public string? EventPrefix { get; set; }
    public float ScalingFactor { get; set; }
    public float CumulativeWeight { get; set; }
    public bool IsUsed { get; set; }
    public bool IsVisible { get; set; }
    public bool ListenerInRange { get; set; }
    public bool Playing2DSound { get; set; }
    public bool PlayingVitalSound { get; set; }
    public float AdditionalCullingWeight { get; set; }
    public float DistanceFromListener { get; set; }
    public bool ForceCullingState { get; set; }
    public float MaxAttenuationRadius { get; set; }
}
