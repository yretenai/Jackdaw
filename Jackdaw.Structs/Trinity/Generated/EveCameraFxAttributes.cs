/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveCameraFxAttributes : IEveFxAttribute {
    public float DistanceToCamera { get; set; }
    public float LookAngleToObject { get; set; }
    [BlackArraySize(3)] public float[]? ObjectRotation { get; set; }
    [BlackArraySize(3)] public float[]? RotationWithChildTransform { get; set; }
    [BlackArraySize(3)] public float[]? CameraRotation { get; set; }
    public string? Name { get; set; }
}
