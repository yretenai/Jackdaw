/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveLocalPositionCurve : ITriVectorFunction {
    public int Behavior { get; set; }
    public ITriVectorFunction? ParentPositionCurve { get; set; }
    public ITriQuaternionFunction? ParentRotationCurve { get; set; }
    public ITriVectorFunction? AlignPositionCurve { get; set; }
    public IEveSpaceObject2? Parent { get; set; }
    public EveTurretSet? TurretSetObject { get; set; }
    public int MuzzleIndex { get; set; }
    [BlackArraySize(3)] public float[]? Value { get; set; }
    [BlackArraySize(3)] public float[]? BoundingSize { get; set; }
    public float Offset { get; set; }
    public string? LocatorSetName { get; set; }
    public int LocatorIndex { get; set; }
    [BlackArraySize(3)] public float[]? PositionOffset { get; set; }
    public int DamageLocatorIndex { get; set; }
    public float ImpactSize { get; set; }
}
