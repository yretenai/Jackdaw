/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveChildEffectPropagator : EveChildContainer, EveEntity, IEveSpaceObjectChild {
    public EveChildInstanceContainer? Effect { get; set; }
    public Tr2CurveScalar? TriggerSphereRadiusCurve { get; set; }
    public EveLocatorSets? LocalLocators { get; set; }
    public bool Trigger { get; set; }
    public bool IsPlaying { get; set; }
    public float PlayTime { get; set; }
    public float TriggerSphereScalarMulti { get; set; }
    public float Completeness { get; set; }
    [BlackArraySize(3)] public float[]? TriggerSphereOffset { get; set; }
    [BlackArraySize(3)] public float[]? EffectScaling { get; set; }
    public float RandScaleMin { get; set; }
    public float RandScaleMax { get; set; }
    public float StopToClearDelay { get; set; }
    public bool SkipCleanup { get; set; }
    public bool ReplayAfterDelay { get; set; }
    public long NumTriggers { get; set; }
    public float Range { get; set; }
    public float MinRangeThreshold { get; set; }
    public float ClosenessPreference { get; set; }
    public string? LocatorSetName { get; set; }
    public float Frequency { get; set; }
    public float DurationPerEffect { get; set; }
    public float StopAfterNumTriggers { get; set; }
    public int PropagationType { get; set; }
    public int TriggerMethood { get; set; }
}
