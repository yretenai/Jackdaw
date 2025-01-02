/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveMissile : EveSpaceObject2, IEveSpaceObject2, ITr2Renderable {
    public object[]? Warheads { get; set; }
    public bool UpdateWarheads { get; set; }
    public ITriTargetable? Target { get; set; }
    public float TargetRadius { get; set; }
    [BlackExperimental] public object? ExplosionCallback { get; set; }
}
