namespace Jackdaw.Structs.Trinity;

[AttributeUsage(AttributeTargets.Class)]
public sealed class BlackObjectAttribute(string name) : Attribute {
	public string Name { get; } = name;

	public override bool Equals(object? obj) => obj is BlackObjectAttribute attribute && Name == attribute.Name;

	public override int GetHashCode() => Name.GetHashCode(StringComparison.InvariantCulture);

	public override bool IsDefaultAttribute() => string.IsNullOrEmpty(Name);
}
