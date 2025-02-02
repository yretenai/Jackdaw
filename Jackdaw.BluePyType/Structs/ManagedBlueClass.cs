using Jackdaw.Blue;

namespace Jackdaw.BluePyType.Structs;

public class ManagedBlueClass : Blue.BlueClass {
	public ManagedBlueClass(BlueClass cls) {
		Hash = cls.Hash;
		Id = cls.Id.Read().GetFullName();
		var pyType = cls.PyType.Read().Read();
		PythonId = pyType.Id.Read().GetFullName();
		ClassId = pyType.ClassId.Read().Read().GetFullName();
		Description = pyType.Description.ReadString();
		Parent = pyType.Parent.Read().Read().GetFullName();

		var index = 0;
		string lastInterfaceType;
		do {
			var link = pyType.Interfaces.Read(index++);
			lastInterfaceType = link.Id.Read().Read().GetFullName();
			if (lastInterfaceType != ClassId && lastInterfaceType != Id && lastInterfaceType.Length > 0) {
				Interfaces.Add(new BlueInterface(lastInterfaceType, link.Flags));
			}
		} while (lastInterfaceType.Length > 0);

		index = 0;
		BluePyField field;
		do {
			field = pyType.Fields.Read(index++);
			if (field.Type != 0) {
				Fields.Add(new ManagedBlueField(field));
			}
		} while (field.Type != 0);
	}
}
