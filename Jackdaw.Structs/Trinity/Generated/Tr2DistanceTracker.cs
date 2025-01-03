/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2DistanceTracker : ITriFunction {
    public bool SignedDistance { get; set; }
    public bool DistanceToClosest { get; set; }
    [BlackArraySize(3)] public float[]? Direction { get; set; }
    public ITriVectorFunction? SourceObject { get; set; }
    public ITriVectorFunction? TargetObject { get; set; }
    [BlackArraySize(3)] public float[]? SourcePosition { get; set; }
    [BlackArraySize(3)] public float[]? TargetPosition { get; set; }
    [BlackUseNamePool] public string? Name { get; set; }
    public float Value { get; set; }
}
