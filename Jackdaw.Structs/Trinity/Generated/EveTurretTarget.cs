/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveTurretTarget {
    public int Locator { get; set; }
    [BlackArraySize(3)] public float[]? Position { get; set; }
    [BlackArraySize(3)] public float[]? TargetPosition { get; set; }
    [BlackArraySize(3)] public float[]? PositionOld { get; set; }
    public float PositionOldInfluence { get; set; }
    public int Behaviour { get; set; }
}
