namespace Jackdaw.Structs.FSD;

public enum FSDStructType {
	Object,
	Array,
	Dictionary,
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class FSDStructAttribute : Attribute {
	public FSDStructAttribute(ulong high, ulong low, FSDStructType type = FSDStructType.Array) {
		High = high;
		Low = low;
		Type = type;
	}

	public ulong High { get; }
	public ulong Low { get; }
	public FSDStructType Type { get; }

	public override bool Equals(object? obj) =>
		obj is FSDStructAttribute attribute &&
		High == attribute.High &&
		Low == attribute.Low &&
		Type == attribute.Type;

	public override int GetHashCode() => HashCode.Combine(High, Low, Type);

	public override bool IsDefaultAttribute() => High == 0 && Low == 0;
}
