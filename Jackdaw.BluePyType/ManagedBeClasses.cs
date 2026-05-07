using System.Diagnostics;
using System.Runtime.InteropServices;
using Jackdaw.Blue;
using Pluto.IO.DataReader;

namespace Jackdaw.BluePyType;

internal class ManagedBeClasses {
	public ManagedBeClasses() {
		Span<byte> slop = stackalloc byte[0x1100];
		var signature = "BluePyOS/mGenericThunkers\0\0\0\0\0\0\0\0\0\0\0\0"u8;
		nint beClassesAddr = 0;
		foreach (var (moduleStart, moduleSize, moduleName) in IMemoryHandler.Handler.EnumerateModules()) {
			if (Path.GetFileName(moduleName.Replace('\\', '/')) != "blue.dll") {
				continue;
			}

			for (var addr = moduleStart; addr < moduleStart + moduleSize; addr += 0x1000) {
				if (!IMemoryHandler.Handler.ReadBytes(addr, slop, out var read)) {
					break;
				}

				// can we just use the export?
				var pos = FindSignature(slop[..read], signature);
				if (pos > -1) {
					beClassesAddr = addr + pos;
					break;
				}
			}
		}

		var root = new BlueTypes();
		IMemoryHandler.Handler.ReadBytes(beClassesAddr, MemoryMarshal.AsBytes(new Span<BlueTypes>(ref root)), out _);

		foreach (var (_, classRegPtr) in root.Classes.Value.Items()) {
			var classReg = classRegPtr.Value;
			if (classReg.Type.IsNull) {
				continue;
			}

			var classInfo = classReg.Type.Value;
			var cls = new BlueClass {
				Flags = classReg.Flags,
				ClassId = classInfo.ClassId.Value.ToString(),
				Parent = classInfo.ParentClassInfo.IsNull ? null : classInfo.ParentClassInfo.Value.ClassId.Value.ToString(),
				Id = classInfo.IID.Value.ToString(),
				Hash = classInfo.IID.Value.Hash,
				Description = classInfo.Description.Value,
			};

			foreach (var iface in classInfo.InterfaceTable) {
				cls.Interfaces.Add(new BlueInterface(iface.IID.Value.ToString(), iface.Offset));
			}

			foreach (var varEntry in classInfo.MemberTable) {
				var field = new BlueField {
					Name = varEntry.Name,
					Description = varEntry.Description.Value,
					Type = varEntry.Type,
					Offset = varEntry.Offset,
					Size = (long) varEntry.Size,
					ClassType = varEntry.IID.IsNull || varEntry.IID.Value.Name.IsNull ? null : varEntry.IID.Value.ToString(),
					EditFlags = varEntry.EditFlags,
				};

				if (!varEntry.ChooserTablePtr.IsNull) {
					field.Choices = [];
					foreach (var value in varEntry.ChooserTable) {
						var choice = new BlueFieldChoice {
							Name = value.Key,
							Description = value.Description.Value,
						};

						switch (varEntry.Type) {
							case BlueTypeId.Int: {
								choice.Value = value.Value.Int;
								break;
							}
							case BlueTypeId.Single: {
								choice.Value = value.Value.Float;
								break;
							}
							case BlueTypeId.Double: {
								choice.Value = value.Value.Double;
								break;
							}
							case BlueTypeId.Boolean: {
								choice.Value = value.Value.Bool;
								break;
							}
							case BlueTypeId.Long: {
								choice.Value = value.Value.Long;
								break;
							}
							case BlueTypeId.Byte: {
								choice.Value = value.Value.Byte;
								break;
							}
							case BlueTypeId.Short: {
								choice.Value = value.Value.Short;
								break;
							}
							case BlueTypeId.UnsignedInt: {
								choice.Value = value.Value.UInt;
								break;
							}
							case BlueTypeId.UnsignedLong: {
								choice.Value = value.Value.ULong;
								break;
							}
							case BlueTypeId.StdString: {
								Debug.Assert(value.Value.ULong == 0);
								choice.Value = value.Description.ToString();
								choice.Description = null;
								break;
							}
							default: throw new IndexOutOfRangeException($"cannot handle {varEntry.Type:G}");
						}

						field.Choices.Add(choice);
					}
				}

				cls.Fields.Add(field);
			}

			Classes.Add(cls);
		}
	}

	public List<BlueClass> Classes { get; } = [];

	private static int FindSignature(ReadOnlySpan<byte> buffer, ReadOnlySpan<byte> signature) {
		if (signature.Length == 0) {
			return -1;
		}

		for (var ptr = 0; ptr < buffer.Length - signature.Length; ++ptr) {
			var found = true;
			for (var i = 0; i < signature.Length; ++i) {
				var b = signature[i];
				if (b != buffer[ptr + i]) {
					found = false;
					break;
				}
			}

			if (found) {
				return ptr;
			}
		}

		return -1;
	}
}
