/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveChildLineSet : IEveSpaceObjectChild, ITr2Renderable {
    public bool AlwaysOn { get; set; }
    public float CurrentScreenSize { get; set; }
    public bool AdditiveBatches { get; set; }
    public float ScrollSpeed { get; set; }
    public float Brightness { get; set; }
    [BlackArraySize(4)] public float[]? BaseColor { get; set; }
    [BlackArraySize(4)] public float[]? AnimColor { get; set; }
    public object[]? Lines { get; set; }
    public EveCurveLineSet? LineSet { get; set; }
    public Tr2Mesh? Mesh { get; set; }
    public string? Name { get; set; }
    public bool Display { get; set; }
    [BlackArraySize(3)] public float[]? Translation { get; set; }
    [BlackArraySize(4)] public float[]? Rotation { get; set; }
    public int RenderType { get; set; }
    public float MinScreenSize { get; set; }
}
