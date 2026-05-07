using System.Runtime.InteropServices;
using Pluto.IO.DataReader;
using Jackdaw.BluePyType.Structs;

namespace Jackdaw.BluePyType;

internal class BeClasses {
	public BeClasses() {
		Span<byte> slop = stackalloc byte[0x1000];
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
					beClassesAddr = addr + pos - 8;

					break;
				}
			}
		}

		var root = new BlueClasses();
		IMemoryHandler.Handler.ReadBytes(beClassesAddr, MemoryMarshal.AsBytes(new Span<BlueClasses>(ref root)), out _);
		var handledAddresses = new HashSet<nint> {
			root.Classes.Address,
		};

		HandleClassLinks(root.Classes.Address.Read<BlueListNode>(), handledAddresses);
	}

	public List<ManagedBlueClass> Classes { get; } = [];

	private void HandleClassLinks(BlueListNode node, HashSet<nint> handledAddresses) {
		if (handledAddresses.Add(node.Left)) {
			var cls = node.Left.Read<BlueClass>();
			Classes.Add(new ManagedBlueClass(cls));
			HandleClassLinks(cls.Node, handledAddresses);
		}

		if (handledAddresses.Add(node.Right)) {
			var cls = node.Right.Read<BlueClass>();
			Classes.Add(new ManagedBlueClass(cls));
			HandleClassLinks(cls.Node, handledAddresses);
		}

		if (handledAddresses.Add(node.Up)) {
			var cls = node.Up.Read<BlueClass>();
			Classes.Add(new ManagedBlueClass(cls));
			HandleClassLinks(cls.Node, handledAddresses);
		}
	}

	private static int FindSignature(Span<byte> buffer, ReadOnlySpan<byte> signature) {
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
