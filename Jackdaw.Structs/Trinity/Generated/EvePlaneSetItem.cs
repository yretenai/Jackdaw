/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EvePlaneSetItem {
    public string? Name { get; set; }
    [BlackArraySize(3)] public float[]? Position { get; set; }
    [BlackArraySize(3)] public float[]? Scaling { get; set; }
    [BlackArraySize(4)] public float[]? Rotation { get; set; }
    [BlackArraySize(4)] public float[]? Color { get; set; }
    [BlackArraySize(4)] public float[]? Layer1Transform { get; set; }
    [BlackArraySize(4)] public float[]? Layer2Transform { get; set; }
    [BlackArraySize(4)] public float[]? Layer1Scroll { get; set; }
    [BlackArraySize(4)] public float[]? Layer2Scroll { get; set; }
    public int BoneIndex { get; set; }
    public int MaskAtlasID { get; set; }
}