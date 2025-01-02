/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveMissileWarhead : EveTransform {
    public float WarheadLength { get; set; }
    public float WarheadRadius { get; set; }
    public float DurationEjectPhase { get; set; }
    public float StartEjectVelocity { get; set; }
    public float Acceleration { get; set; }
    public float MaxExplosionDistance { get; set; }
    public float ImpactSize { get; set; }
    public float ImpactDuration { get; set; }
    public int Id { get; set; }
    [BlackArraySize(3)] public float[]? ExplosionPosition { get; set; }
    public bool DoSpread { get; set; }
    public int TargetLocatorID { get; set; }
    public EveSpriteSet? SpriteSet { get; set; }
    public bool StartDataValid { get; set; }
    [BlackArraySize(3)] public float[]? PathOffset { get; set; }
    public float PathOffsetNoiseScale { get; set; }
    public float PathOffsetNoiseSpeed { get; set; }
}
