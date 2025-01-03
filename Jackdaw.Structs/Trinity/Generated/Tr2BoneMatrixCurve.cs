/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2BoneMatrixCurve : ITriFunction, ITriCurveLength {
    public object[]? Keys { get; set; }
    public string? Bone { get; set; }
    [BlackArraySize(16)] public float[]? Transform { get; set; }
    public string? Name { get; set; }
    public float Length { get; set; }
    public bool Cycle { get; set; }
    public bool Reversed { get; set; }
    [BlackArraySize(16)] public float[]? StartValue { get; set; }
    [BlackArraySize(16)] public float[]? CurrentValue { get; set; }
    [BlackArraySize(16)] public float[]? EndValue { get; set; }
    public Tr2SkinnedObject? SkinnedObject { get; set; }
}
