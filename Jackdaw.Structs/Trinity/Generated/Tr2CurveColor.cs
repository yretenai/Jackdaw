/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2CurveColor : ITriColorFunction, ITriFunction, ITriCurveLength {
    public bool SrgbOutput { get; set; }
    [BlackPureRef] public Tr2CurveScalar? R { get; set; }
    [BlackPureRef] public Tr2CurveScalar? G { get; set; }
    [BlackPureRef] public Tr2CurveScalar? B { get; set; }
    [BlackPureRef] public Tr2CurveScalar? A { get; set; }
    public float TimeOffset { get; set; }
    [BlackArraySize(4)] public float[]? CurrentValue { get; set; }
    public string? Name { get; set; }
}
