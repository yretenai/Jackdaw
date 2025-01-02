/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveProjectBracket : ITriFunction {
    [BlackUseNamePool] public string? Name { get; set; }
    public ITriVectorFunction? TrackBall { get; set; }
    public float BallTrackingScaling { get; set; }
    public IWorldPosition? TrackTransform { get; set; }
    [BlackArraySize(3)] public float[]? TrackPosition { get; set; }
    public Tr2Sprite2dContainer? Bracket { get; set; }
    public EveSprite2dBracket? BracketIcon { get; set; }
    public Tr2Sprite2dContainer? Parent { get; set; }
    public bool Dock { get; set; }
    public float MinDispRange { get; set; }
    public float MaxDispRange { get; set; }
    public float CameraDistance { get; set; }
    public float OffsetX { get; set; }
    public float OffsetY { get; set; }
    public bool IntegerCoordinates { get; set; }
    [BlackArraySize(2)] public float[]? ProjectedPosition { get; set; }
    [BlackArraySize(2)] public float[]? RawProjectedPosition { get; set; }
    [BlackExperimental] public object? DisplayChangeCallback { get; set; }
    [BlackExperimental] public object? BracketUpdateCallback { get; set; }
    public bool IsVisible { get; set; }
    public bool IsInFront { get; set; }
    public float MarginLeft { get; set; }
    public float MarginRight { get; set; }
    public float MarginTop { get; set; }
    public float MarginBottom { get; set; }
}