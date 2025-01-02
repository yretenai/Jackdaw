/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveBoxVolume : IEveVolume {
    public bool DebugShowIntersection { get; set; }
    public string? Name { get; set; }
    [BlackArraySize(3)] public float[]? Position { get; set; }
    [BlackArraySize(3)] public float[]? Scaling { get; set; }
    [BlackArraySize(3)] public float[]? InnerScaling { get; set; }
    [BlackArraySize(4)] public float[]? Rotation { get; set; }
}