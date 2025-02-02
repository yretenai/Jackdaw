using Jackdaw.Blue;

namespace Jackdaw.BluePyType.Structs;

public class ManagedBlueClass : Blue.BlueClass {
	public ManagedBlueClass(BlueClass cls) {
		Hash = cls.Hash;
		Id = cls.Id.Read<BlueID>().GetFullName();
		var pyType = cls.PyType.Read<nint>().Read<BluePyType>();
		PythonId = pyType.Id.Read<BlueID>().GetFullName();
		ClassId = pyType.ClassId.Read<BlueCLSID>().Read().GetFullName();
		Description = pyType.Description.ReadString();
		Parent = pyType.Parent.Read<nint>().Read<BlueID>().GetFullName();

		var index = 0;
		string lastInterfaceType;
		do {
			var link = pyType.Interfaces.Read<BlueCLSIDLink>(index++);
			lastInterfaceType = link.Id.Read<BlueCLSID>().Read().GetFullName();
			if (lastInterfaceType != ClassId && lastInterfaceType != Id && lastInterfaceType.Length > 0) {
				Interfaces.Add(new BlueInterface(lastInterfaceType, link.Flags));
			}
		} while (lastInterfaceType.Length > 0);

		index = 0;
		BluePyField field;
		do {
			field = pyType.Fields.Read<BluePyField>(index++);
			if (field.Type != 0) {
				Fields.Add(new ManagedBlueField(field));
			}
		} while (field.Type != 0);
	}
}
