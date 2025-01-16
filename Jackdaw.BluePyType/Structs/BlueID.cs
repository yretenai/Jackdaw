using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public record struct BlueID {
	public BluePtr<byte> Namespace { get; set; }
	public BluePtr<byte> Name { get; set; }
	public uint Hash { get; set; }

	public void GetValues(out string ns, out string name) {
		name = Name.ReadString();

		if (Namespace.IsZero) {
			var dot = name.IndexOf('.', StringComparison.Ordinal);
			if (dot > 0) {
				ns = name[..dot];
				name = name[(dot + 1)..];
			} else {
				ns = string.Empty;
			}
		} else {
			ns = Namespace.ReadString();
		}

		if (name.Length == 0 && ns.Length > 0) {
			var dot = ns.IndexOf('.', StringComparison.Ordinal);
			if (dot > 0) {
				name = ns[(dot + 1)..];
				ns = ns[..dot];
			} else {
				(ns, name) = (name, ns);
			}
		}
	}

	public string GetFullName() {
		GetValues(out var ns, out var name);
		return ns.Length == 0 ? name : $"{ns}.{name}";
	}
}
