/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveVirtualCamera {
    public string? Name { get; set; }
    public float AnimationTimelineLength { get; set; }
    public bool Running { get; set; }
    public float Fov { get; set; }
    public float Roll { get; set; }
    [BlackArraySize(3)] public float[]? Position { get; set; }
    [BlackArraySize(3)] public float[]? PointOfInterest { get; set; }
    public float LocalElapsedTime { get; set; }
    [BlackArraySize(3)] public float[]? PositionAnchorCenter { get; set; }
    public float PositionAnchorRadius { get; set; }
    [BlackArraySize(3)] public float[]? PositionAnchorForwardDirection { get; set; }
    [BlackArraySize(3)] public float[]? PointOfInterestAnchorCenter { get; set; }
    public float PointOfInterestAnchorRadius { get; set; }
    [BlackArraySize(3)] public float[]? PointOfInterestAnchorForwardDirection { get; set; }
    [BlackArraySize(3)] public float[]? Forward { get; set; }
    [BlackArraySize(3)] public float[]? Right { get; set; }
    [BlackArraySize(3)] public float[]? Up { get; set; }
    public object[]? PositionBehaviours { get; set; }
    public object[]? PointOfInterestBehaviours { get; set; }
    public object[]? FovBehaviours { get; set; }
    public object[]? RollBehaviours { get; set; }
    public object[]? PositionAnchors { get; set; }
    public object[]? PointOfInterestAnchors { get; set; }
}