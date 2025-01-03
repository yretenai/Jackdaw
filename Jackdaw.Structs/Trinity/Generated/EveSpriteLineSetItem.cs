/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveSpriteLineSetItem {
    public int BoneIndex { get; set; }
    public string? Name { get; set; }
    public bool IsCircle { get; set; }
    [BlackArraySize(3)] public float[]? Position { get; set; }
    [BlackArraySize(4)] public float[]? Rotation { get; set; }
    [BlackArraySize(3)] public float[]? Scaling { get; set; }
    public float Spacing { get; set; }
    public float BlinkRate { get; set; }
    public float BlinkPhase { get; set; }
    public float BlinkPhaseShift { get; set; }
    public float MinScale { get; set; }
    public float MaxScale { get; set; }
    public float Falloff { get; set; }
    [BlackArraySize(4)] public float[]? Color { get; set; }
}
