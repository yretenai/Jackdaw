/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveSOFDataHullHazeSetItem {
    public int BoneIndex { get; set; }
    [BlackArraySize(3)] public float[]? Position { get; set; }
    [BlackArraySize(3)] public float[]? Scaling { get; set; }
    [BlackArraySize(4)] public float[]? Rotation { get; set; }
    public int ColorType { get; set; }
    public float HazeBrightness { get; set; }
    public float HazeFalloff { get; set; }
    public float SourceBrightness { get; set; }
    public float SourceSize { get; set; }
    public bool BoosterGainInfluence { get; set; }
}