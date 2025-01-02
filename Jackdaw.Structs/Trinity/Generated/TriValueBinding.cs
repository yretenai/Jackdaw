/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class TriValueBinding : ITr2ValueBinding {
    public string? Name { get; set; }
    public bool IsValid { get; set; }
    public bool IsEnabled { get; set; }
    public object? SourceObject { get; set; }
    public string? SourceAttribute { get; set; }
    public object? DestinationObject { get; set; }
    public string? DestinationAttribute { get; set; }
    public float Scale { get; set; }
    [BlackArraySize(4)] public float[]? Offset { get; set; }
    [BlackExperimental] public object? CopyValueCallable { get; set; }
}
