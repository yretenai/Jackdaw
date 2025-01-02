/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2PPVignetteEffect : Tr2PPEffect {
    public string? ShapePath { get; set; }
    public string? DetailPath { get; set; }
    [BlackArraySize(2)] public float[]? Detail1Size { get; set; }
    [BlackArraySize(2)] public float[]? Detail2Size { get; set; }
    [BlackArraySize(2)] public float[]? Detail1Scroll { get; set; }
    [BlackArraySize(2)] public float[]? Detail2Scroll { get; set; }
    [BlackArraySize(4)] public float[]? Color { get; set; }
    public float Opacity { get; set; }
    public float Intensity { get; set; }
    public float SineFrequency { get; set; }
    public float SineMinimum { get; set; }
    public float SineMaximum { get; set; }
}