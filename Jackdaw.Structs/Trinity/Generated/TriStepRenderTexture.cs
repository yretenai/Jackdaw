/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class TriStepRenderTexture : TriRenderStep {
    public ITr2TextureProvider? Texture { get; set; }
    public ITr2TextureProvider? RenderTarget { get; set; }
    public ITr2TextureProvider? DepthStencil { get; set; }
    [BlackArraySize(2)] public float[]? TlTexCoord { get; set; }
    [BlackArraySize(2)] public float[]? BrTexCoord { get; set; }
    [BlackArraySize(2)] public float[]? TextureSize { get; set; }
    public int FailClearColor { get; set; }
}