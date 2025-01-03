using System.Runtime.CompilerServices;

namespace Knit.Meta;

[InlineArray(4)]
public struct Granny2Magic : IEquatable<Granny2Magic> {
	public uint Value;

	public Granny2Magic(uint a, uint b, uint c, uint d) {
		this[0] = a;
		this[1] = b;
		this[2] = c;
		this[3] = d;
	}

	public override int GetHashCode() {
		var hashCode = new HashCode();
		hashCode.Add(this[0]);
		hashCode.Add(this[1]);
		hashCode.Add(this[2]);
		hashCode.Add(this[3]);
		return hashCode.ToHashCode();
	}

	public bool Equals(Granny2Magic other) => Equals((ReadOnlySpan<uint>) other);
	public bool Equals(Span<uint> other) => other.SequenceEqual(this);
	public bool Equals(ReadOnlySpan<uint> other) => other.SequenceEqual(this);
	public override bool Equals(object? obj) => obj is Granny2Magic other && Equals(other);
	public static bool operator ==(Granny2Magic left, Granny2Magic right) => left.Equals(right);
	public static bool operator !=(Granny2Magic left, Granny2Magic right) => !(left == right);
	public static bool operator ==(Granny2Magic left, Span<uint> right) => left.Equals(right);
	public static bool operator !=(Granny2Magic left, Span<uint> right) => !(left == right);
	public static bool operator ==(Granny2Magic left, ReadOnlySpan<uint> right) => left.Equals(right);
	public static bool operator !=(Granny2Magic left, ReadOnlySpan<uint> right) => !(left == right);
}
