/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveSOFDataHullBanner {
    public int Usage { get; set; }
    public string? VisibilityGroup { get; set; }
    public int BoneIndex { get; set; }
    public bool MaintainAspectRatio { get; set; }
    public string? Name { get; set; }
    [BlackArraySize(3)] public float[]? Position { get; set; }
    [BlackArraySize(3)] public float[]? Scaling { get; set; }
    [BlackArraySize(4)] public float[]? Rotation { get; set; }
    public float AngleX { get; set; }
    public float AngleY { get; set; }
    public EveSOFDataHullBannerLight? LightOverride { get; set; }
}