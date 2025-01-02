/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2GeometryBufferParameter : ITriEffectParameter, ITriEffectResourceParameter {
    public string? Name { get; set; }
    [BlackUseNamePool] public string? ResourcePath { get; set; }
    public int MeshIndex { get; set; }
    public bool IsValid { get; set; }
    public ITr2GpuBuffer? GpuBuffer { get; set; }
    public bool UsedByCurrentEffect { get; set; }
}