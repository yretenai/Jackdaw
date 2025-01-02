/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2CurveLineSet : ITr2Renderable, ITr2Pickable {
    public Tr2Material? LineEffect { get; set; }
    public Tr2Material? PickEffect { get; set; }
    public float LineWidthFactor { get; set; }
    public string? Name { get; set; }
    public float DepthOffset { get; set; }
    public bool Additive { get; set; }
    [BlackArraySize(3)] public float[]? Translation { get; set; }
    [BlackArraySize(4)] public float[]? Rotation { get; set; }
    [BlackArraySize(3)] public float[]? Scaling { get; set; }
    public bool Display { get; set; }
}