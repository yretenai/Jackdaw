namespace Jackdaw.Structs.Trinity;

[AttributeUsage(AttributeTargets.Property)]
public sealed class BlackArrayAttribute(int sizeOfEntry = 0) : Attribute {
	public int Size { get; } = sizeOfEntry;
	public override bool Equals(object? obj) => obj is BlackArrayAttribute other && Size == other.Size;
	public override int GetHashCode() => Size;
	public override bool IsDefaultAttribute() => Size == 0;
}
