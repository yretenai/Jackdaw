namespace Jackdaw.Blue;

public class BlueClass {
	public uint Hash { get; set; }
	public string Id { get; set; } = string.Empty;
	public string PythonId { get; set; } = string.Empty;
	public string ClassId { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string Parent { get; set; } = string.Empty;
	public List<BlueInterface> Interfaces { get; set; } = [];
	public List<BlueField> Fields { get; set; } = [];

	public override string ToString() => Id;
}
