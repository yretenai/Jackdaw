using System.Runtime.CompilerServices;

namespace Knit.Meta;

[InlineArray(4)]
public struct Granny2ExtraTags : IEquatable<Granny2ExtraTags>, IEquatable<Span<uint>>, IEquatable<ReadOnlySpan<uint>>, IEquatable<uint> {
	public uint Value;

	public override int GetHashCode() {
		var hashCode = new HashCode();
		hashCode.Add(this[0]);
		hashCode.Add(this[1]);
		hashCode.Add(this[2]);
		hashCode.Add(this[3]);
		return hashCode.ToHashCode();
	}

	public bool Equals(Granny2ExtraTags other) => Equals((ReadOnlySpan<uint>) other);
	public bool Equals(Span<uint> other) => other.SequenceEqual(this);
	public bool Equals(ReadOnlySpan<uint> other) => other.SequenceEqual(this);
	public bool Equals(uint other) => ((ReadOnlySpan<uint>) this).Contains(other);
	public override bool Equals(object? obj) => obj is Granny2ExtraTags other && Equals(other);
	public static bool operator ==(Granny2ExtraTags left, Granny2ExtraTags right) => left.Equals(right);
	public static bool operator !=(Granny2ExtraTags left, Granny2ExtraTags right) => !(left == right);
	public static bool operator ==(Granny2ExtraTags left, Span<uint> right) => left.Equals(right);
	public static bool operator !=(Granny2ExtraTags left, Span<uint> right) => !(left == right);
	public static bool operator ==(Granny2ExtraTags left, ReadOnlySpan<uint> right) => left.Equals(right);
	public static bool operator !=(Granny2ExtraTags left, ReadOnlySpan<uint> right) => !(left == right);
}
