/// <auto-generated/>
#nullable enable

namespace Jackdaw.Structs.Trinity.Generated;

public class Tr2ActionAnimateValue : ITr2ControllerAction, ITr2Updateable {
    public string? Path { get; set; }
    public object? Destination { get; set; }
    public string? Attribute { get; set; }
    public string? Value { get; set; }
    public ITriScalarFunction? Curve { get; set; }
    public bool DelayBinding { get; set; }
    public bool IsBindingValid { get; set; }
    public bool IsExpressionValid { get; set; }
}
