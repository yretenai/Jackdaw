using Jackdaw.Blue;

namespace Jackdaw.BluePyType.Structs;

public record ManagedBlueField : BlueField {
	public ManagedBlueField(BluePyField field) {
		Name = field.Name.ReadString();
		Description = field.Description.ReadString();
		ClassType = field.ClassType.Read().Read().GetFullName();
		Type = (BlueTypeId) field.Type;
		Offset = field.Offset;
		Size = field.Size;
		TypeId = field.TypeId;
	}
}
