/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2InteriorLightSource : ITr2InteriorLight {
    public string? Name { get; set; }
    [BlackArraySize(3)] public float[]? Position { get; set; }
    public float Radius { get; set; }
    [BlackArraySize(4)] public float[]? Color { get; set; }
    public Tr2KelvinColor? KelvinColor { get; set; }
    public bool UseKelvinColor { get; set; }
    public float Falloff { get; set; }
    public float SpecularIntensity { get; set; }
    public float ConeAlphaOuter { get; set; }
    public float ConeAlphaInner { get; set; }
    [BlackArraySize(3)] public float[]? ConeDirection { get; set; }
    public bool PrimaryLighting { get; set; }
    public object[]? CurveSets { get; set; }
}