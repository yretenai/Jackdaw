/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveSpherePin : ITr2Renderable, IEveTransform, IEveSpaceObject2, ITr2Pickable {
    public string? Name { get; set; }
    public Tr2Effect? PinEffect { get; set; }
    public Tr2Effect? PickEffect { get; set; }
    public object[]? CurveSets { get; set; }
    public string? GeometryResPath { get; set; }
    public string? PinEffectResPath { get; set; }
    public float SortValueMultiplier { get; set; }
    [BlackArraySize(4)] public float[]? UvAtlasScaleOffset { get; set; }
    [BlackArraySize(3)] public float[]? CenterNormal { get; set; }
    public float PinRadius { get; set; }
    public float PinMaxRadius { get; set; }
    public float PinRotation { get; set; }
    [BlackArraySize(4)] public float[]? PinColor { get; set; }
    [BlackArraySize(4)] public float[]? Color { get; set; }
    public float PinAlphaThreshold { get; set; }
    [BlackArraySize(3)] public float[]? Translation { get; set; }
    [BlackArraySize(4)] public float[]? Rotation { get; set; }
    [BlackArraySize(3)] public float[]? Scaling { get; set; }
    public bool Display { get; set; }
    public bool EnablePicking { get; set; }
    public int PrimitiveCount { get; set; }
}
