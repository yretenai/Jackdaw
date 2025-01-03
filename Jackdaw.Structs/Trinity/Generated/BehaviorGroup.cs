/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class BehaviorGroup {
    public string? Name { get; set; }
    public bool Update { get; set; }
    public int Count { get; set; }
    public int ActualCount { get; set; }
    [BlackArraySize(3)] public float[]? SpawnPosition { get; set; }
    public object[]? Behaviors { get; set; }
    public Tr2Mesh? Mesh { get; set; }
    public BehaviorGroupBooster? Boosters { get; set; }
    public float Scale { get; set; }
    public float CurrentScreenSize { get; set; }
    public float RenderThreshold { get; set; }
    public float BlendScreenSizeMin { get; set; }
    public float BlendScreenSizeMax { get; set; }
    public float BoundingSphereRadius { get; set; }
    public bool DebugMode { get; set; }
    public float DebugLodLevel { get; set; }
    public float DebugIntensity { get; set; }
    public bool Display { get; set; }
    public float MaxVelocity { get; set; }
}
