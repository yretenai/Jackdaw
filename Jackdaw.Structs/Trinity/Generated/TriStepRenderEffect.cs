/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class TriStepRenderEffect : TriRenderStep {
    public Tr2Effect? Effect { get; set; }
    public Tr2ShaderBuffer? ShaderBuffer { get; set; }
    [BlackArraySize(2)] public float[]? TlTexCoord { get; set; }
    [BlackArraySize(2)] public float[]? BrTexCoord { get; set; }
}
