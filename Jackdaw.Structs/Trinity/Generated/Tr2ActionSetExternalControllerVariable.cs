/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2ActionSetExternalControllerVariable : ITr2ControllerAction {
    public string? DestinationOwner { get; set; }
    [BlackExperimental] public object? Destination { get; set; }
    public string? Variable { get; set; }
    public float Value { get; set; }
    public string? SourceVariable { get; set; }
    public bool StartControllers { get; set; }
    public bool DestinationIsValid { get; set; }
}
