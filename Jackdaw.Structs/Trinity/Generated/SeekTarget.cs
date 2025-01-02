/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class SeekTarget : IBehavior {
    public int BehaviorPriority { get; set; }
    public bool Enabled { get; set; }
    public EveLocatorSets? LocatorSet { get; set; }
    public string? LocatorSetName { get; set; }
    public float SecondsToTurn { get; set; }
    public bool Exit { get; set; }
    public bool Repair { get; set; }
    public float BehaviorWeight { get; set; }
    public float DistFromOrigin { get; set; }
    public float ArrivedRadius { get; set; }
    public float SlowDownRadius { get; set; }
    public EveSpaceObject2? Target { get; set; }
    [BlackExperimental] public object? OnFirstDroneArrivedCallback { get; set; }
    public float TotalRepairTime { get; set; }
    public bool FirstSpawnAtRandomPlaces { get; set; }
}