/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2CurveColorMixer : ITriColorFunction, ITriFunction, ITriCurveLength {
    public float LerpValue { get; set; }
    public float Saturation { get; set; }
    public float Brightness { get; set; }
    [BlackArraySize(4)] public float[]? CurrentValue { get; set; }
    public string? Name { get; set; }
    [BlackArraySize(4)] public float[]? Color1 { get; set; }
    [BlackArraySize(4)] public float[]? Color2 { get; set; }
}