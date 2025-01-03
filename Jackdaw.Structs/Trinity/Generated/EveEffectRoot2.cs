/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveEffectRoot2 : EveSpaceObject2, EveEntity, IEveSpaceObject2, ITr2SecondaryLightSource, ITriTargetable, ITr2CurveSetOwner, IEveEffectChildrenOwner, ITr2ControllerOwner, IShaderConfigurer, ITr2SoundEmitterOwner, ITr2LightOwner, IWorldPosition {
    [BlackArraySize(4)] public float[]? SecondaryLightingEmissiveColor { get; set; }
    public float EstimatedSize { get; set; }
    public bool DynamicLOD { get; set; }
    [BlackArraySize(3)] public float[]? Scaling { get; set; }
    [BlackArraySize(4)] public float[]? Rotation { get; set; }
    [BlackArraySize(3)] public float[]? Translation { get; set; }
    public float Duration { get; set; }
}
