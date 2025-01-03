/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class TriObserverLocal : ITriObserverLocal {
    public string? Name { get; set; }
    [BlackArraySize(3)] public float[]? Position { get; set; }
    [BlackArraySize(3)] public float[]? Front { get; set; }
    public IBluePlacementObserver? Observer { get; set; }
}
