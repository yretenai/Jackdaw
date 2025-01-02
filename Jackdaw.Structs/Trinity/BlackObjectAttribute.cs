namespace Jackdaw.Structs.Trinity;

[AttributeUsage(AttributeTargets.Class)]
public sealed class BlackObjectAttribute : Attribute {
	public BlackObjectAttribute(string name) => Name = name;
	public string Name { get; }

	public override bool Equals(object? obj) => obj is BlackObjectAttribute attribute && Name == attribute.Name;

	public override int GetHashCode() => Name.GetHashCode(StringComparison.InvariantCulture);

	public override bool IsDefaultAttribute() => string.IsNullOrEmpty(Name);
}
