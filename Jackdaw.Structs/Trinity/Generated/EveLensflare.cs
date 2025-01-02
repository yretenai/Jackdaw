/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveLensflare : ITr2ControllerOwner, ITr2CurveSetOwner {
    public string? Name { get; set; }
    public bool Display { get; set; }
    public bool Update { get; set; }
    public bool DoOcclusionQueries { get; set; }
    public float CameraFactor { get; set; }
    public Tr2Mesh? Mesh { get; set; }
    [BlackArraySize(3)] public float[]? Position { get; set; }
    public object[]? Flares { get; set; }
    public object[]? Occluders { get; set; }
    public object[]? BackgroundOccluders { get; set; }
    public float OcclusionIntensity { get; set; }
    public object[]? DistanceToEdgeCurves { get; set; }
    public object[]? DistanceToCenterCurves { get; set; }
    public object[]? RadialAngleCurves { get; set; }
    public object[]? XDistanceToCenter { get; set; }
    public object[]? YDistanceToCenter { get; set; }
    public object[]? Bindings { get; set; }
    public object[]? CurveSets { get; set; }
    public object[]? Controllers { get; set; }
    public ITriVectorFunction? TranslationCurve { get; set; }
}