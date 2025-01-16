namespace Jackdaw.BluePyType.Structs;

public record struct BlueCLSIDLink {
	public BluePtr<BlueCLSID> Id { get; set; }
	public ulong Flags { get; set; }
}
