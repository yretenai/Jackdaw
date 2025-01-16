namespace Jackdaw.BluePyType.Structs;

public record ManagedBlueField {
	public ManagedBlueField(BluePyField field) {
		Name = field.Name.ReadString();
		Description = field.Description.ReadString();
		ClassType = field.ClassType.Read().Read().GetFullName();
		Type = field.Type;
		Offset = field.Offset;
		Size = field.Size;
		TypeId = field.TypeId;
	}

	public string Name { get; set; }
	public string Description { get; set; }
	public long Type { get; set; }
	public long Offset { get; set; }
	public long Size { get; set; }
	public string ClassType { get; set; }
	public ulong TypeId { get; set; }
}
