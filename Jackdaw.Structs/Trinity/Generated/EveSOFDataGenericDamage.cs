/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class EveSOFDataGenericDamage {
    public float FlickerPerlinSpeed { get; set; }
    public float FlickerPerlinAlpha { get; set; }
    public float FlickerPerlinBeta { get; set; }
    public int FlickerPerlinN { get; set; }
    public float ArmorParticleRate { get; set; }
    public float ArmorParticleAngle { get; set; }
    [BlackArraySize(2)] public float[]? ArmorParticleMinMaxSpeed { get; set; }
    [BlackArraySize(2)] public float[]? ArmorParticleMinMaxLifeTime { get; set; }
    [BlackArraySize(4)] public float[]? ArmorParticleSizes { get; set; }
    [BlackArraySize(4)] public float[]? ArmorParticleColor0 { get; set; }
    [BlackArraySize(4)] public float[]? ArmorParticleColor1 { get; set; }
    [BlackArraySize(4)] public float[]? ArmorParticleColor2 { get; set; }
    [BlackArraySize(4)] public float[]? ArmorParticleColor3 { get; set; }
    public int ArmorParticleTextureIndex { get; set; }
    public float ArmorParticleVelocityStretchRotation { get; set; }
    public float ArmorParticleDrag { get; set; }
    public float ArmorParticleTurbulenceAmplitude { get; set; }
    public int ArmorParticleTurbulenceFrequency { get; set; }
    public float ArmorParticleColorMidPoint { get; set; }
    public string? ArmorShader { get; set; }
    public string? ShieldShaderEllipsoid { get; set; }
    public string? ShieldShaderHull { get; set; }
    public string? ShieldGeometryResFilePath { get; set; }
}