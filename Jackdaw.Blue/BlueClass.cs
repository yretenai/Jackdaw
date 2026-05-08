namespace Jackdaw.Blue;

public class BlueClass {
	public uint Hash { get; set; }
	public string Id { get; set; } = string.Empty;
	public string ClassId { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? Parent { get; set; }
	public BlueClassRegistrationFlags Flags { get; set; }
	public List<BlueInterface> Interfaces { get; set; } = [];
	public List<BlueField> Fields { get; set; } = [];

	public override string ToString() => ClassId;
}

[Flags]
public enum BlueClassRegistrationFlags : uint {
	None = 0,
	DisablePythonConstruction = 1,
}
