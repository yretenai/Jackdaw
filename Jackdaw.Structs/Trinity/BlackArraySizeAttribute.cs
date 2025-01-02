namespace Jackdaw.Structs.Trinity;

[AttributeUsage(AttributeTargets.Property)]
public sealed class BlackArraySizeAttribute : Attribute {
	public BlackArraySizeAttribute(int size) => Size = size;
	public int Size { get; }

	public override bool Equals(object? obj) => obj is BlackArraySizeAttribute attribute && Size == attribute.Size;

	public override int GetHashCode() => Size.GetHashCode();

	public override bool IsDefaultAttribute() => Size == 0;
}
