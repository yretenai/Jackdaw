using System.Runtime.InteropServices;

namespace Jackdaw.BluePyType.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public record struct BlueCLSID {
	public nint Address { get; set; }
	public uint Hash { get; set; }

	public BlueID Read() {
		if (Hash == 0) {
			return Address.Read<BlueID>();
		}

		return new BlueID {
			Name = Address,
			Hash = Hash,
		};
	}
}
