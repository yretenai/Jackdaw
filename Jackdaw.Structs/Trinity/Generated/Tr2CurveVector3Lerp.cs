/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2CurveVector3Lerp : ITriFunction, ITriVectorFunction {
    [BlackArraySize(3)] public float[]? InitialValue { get; set; }
    public int StartInterpolation { get; set; }
    public ITriVectorFunction? Curve { get; set; }
    public float CurveStartTime { get; set; }
    [BlackArraySize(3)] public float[]? CurrentValue { get; set; }
    public string? Name { get; set; }
}
