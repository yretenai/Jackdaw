/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class BackAndForth : IBehavior {
    public bool Enabled { get; set; }
    public int BehaviorPriority { get; set; }
    public int LocatorType { get; set; }
    public string? LocatorSetName { get; set; }
    public float SecondsToTurn { get; set; }
    public float ArrivedRadius { get; set; }
    public float DistFromOrigin { get; set; }
    public float SlowDownRadius { get; set; }
    public float BackAndForthWeight { get; set; }
    public IBehavior? FxBehavior { get; set; }
    public object[]? LocatorSet { get; set; }
    public EveSpaceObject2? Target { get; set; }
    public EveSpaceObject2? Parent { get; set; }
}
