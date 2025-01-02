namespace Jackdaw.Structs.Trinity;

[AttributeUsage(AttributeTargets.Property)]
public sealed class BlackPureRefAttribute : Attribute {
	public BlackPureRefAttribute() => Size = 0;
	public BlackPureRefAttribute(int size) => Size = size;
	public int Size { get; }
	public override bool Equals(object? obj) => obj is BlackPureRefAttribute other && Size == other.Size;
	public override int GetHashCode() => Size;
	public override bool IsDefaultAttribute() => Size == 0;
}
