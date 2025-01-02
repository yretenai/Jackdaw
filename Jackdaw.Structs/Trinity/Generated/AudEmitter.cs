/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class AudEmitter : AudGameObjResource, IBlueEventListener, IBluePlacementObserver, ITr2AudEmitter {
    public bool NormalizeAttenuationScaling { get; set; }
    public float MinNormalizedValue { get; set; }
    public float MaxNormalizedValue { get; set; }
    public float MinNormalizedScalingFactor { get; set; }
    public float MaxNormalizedScalingFactor { get; set; }
    [BlackArraySize(3)] public float[]? DebugPosition { get; set; }
    [BlackArraySize(3)] public float[]? DebugFront { get; set; }
}