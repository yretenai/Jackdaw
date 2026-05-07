using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Jackdaw.Blue;
using Pluto.Extensions;
using Pluto.IO.DataReader;

namespace Jackdaw.BluePyType;

#pragma warning disable CA1815
using MemoryCStrPtr = MemoryStrPtr<byte>;
using MemoryWStrPtr = MemoryStrPtr<ushort>;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueIID {
	public MemoryCStrPtr Name { get; set; }
	public uint Hash { get; set; }

	public override string ToString() => Name;
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueCLSID {
	public MemoryCStrPtr Module { get; set; }
	public MemoryCStrPtr Name { get; set; }
	public uint Hash { get; set; }

	public override string ToString() {
		var module = Module.ToString();
		return string.IsNullOrEmpty(module) ? Name.ToString() : $"{module}.{Name}";
	}
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueGenericThunker {
	public MemoryPtr<BlueMethodDefinition> DefPtrs { get; set; }
	public MemoryPtr<BlueIID> IID { get; set; }

	public IEnumerable<BlueMethodDefinition> Definitions {
		get {
			var idx = 0;
			do {
				if (!DefPtrs.TryRead(idx++, out var def) || def.Name.IsNull) {
					yield break;
				}

				yield return def;
			} while (true);
		}
	}
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueMethodDefinition {
	// PyMethodDef
	public MemoryCStrPtr Name { get; set; }
	public nint Method { get; set; }
	public uint Flags { get; set; } // not here if !BLUE_WITH_PYTHON
	public MemoryCStrPtr Doc { get; set; }
}

// a union, yay.
[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct BlueVar {
	[field: FieldOffset(0)] public double Double { get; set; }
	[field: FieldOffset(0)] public int Int { get; set; }
	[field: FieldOffset(0)] public uint UInt { get; set; }
	[field: FieldOffset(0)] public float Float { get; set; }

	[field: FieldOffset(0)] [field: MarshalAs(UnmanagedType.I1)]
	public bool Bool { get; set; }

	[field: FieldOffset(0)] public byte Byte { get; set; }
	[field: FieldOffset(0)] public short Short { get; set; }
	[field: FieldOffset(0)] public nint BlueObject { get; set; }
	[field: FieldOffset(0)] public MemoryCStrPtr CharPtr { get; set; }
	[field: FieldOffset(0)] public MemoryWStrPtr WideCharPtr { get; set; }
	[field: FieldOffset(0)] public long Long { get; set; }
	[field: FieldOffset(0)] public ulong ULong { get; set; }
	[field: FieldOffset(0)] public nint PyObject { get; set; }
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueVarChooser {
	public MemoryCStrPtr Key { get; set; }
	public BlueVar Value { get; set; }
	public MemoryCStrPtr Description { get; set; }
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueVarEntry {
	public MemoryCStrPtr Name { get; set; }
	public BlueTypeId Type { get; set; }
	public nint Offset { get; set; }
	public nuint Size { get; set; }
	public MemoryPtr<BlueIID> IID { get; set; }
	public MemoryCStrPtr Description { get; set; }
	public BlueVarEditFlags EditFlags { get; set; }
	public MemoryPtr<BlueVarChooser> ChooserTablePtr { get; set; }
	public nint GetProperty { get; set; }
	public nint SetProperty { get; set; }

	public IEnumerable<BlueVarChooser> ChooserTable {
		get {
			var idx = 0;
			do {
				if (!ChooserTablePtr.TryRead(idx++, out var choice) || choice.Key.IsNull) {
					yield break;
				}

				yield return choice;
			} while (true);
		}
	}
}

public struct BlueInterfaceEntry {
	public MemoryPtr<BlueIID> IID { get; set; }
	public nuint Offset { get; set; }
}

[InlineArray(0x40)] [StructLayout(LayoutKind.Sequential)]
public struct BlueTrackerName {
	public byte Value { get; set; }
}

[InlineArray(16)] [StructLayout(LayoutKind.Sequential)]
public struct BlueArgumentTypes {
	public MemoryCStrPtr Value { get; set; }
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueString {
	public MemoryCStrPtr Ptr { get; set; }
	public nuint Storage { get; set; }
	public nuint Length { get; set; }
	public nuint Capacity { get; set; }

	public override string ToString() {
		if (Capacity <= 16) {
			return Ptr.TryReadStr((int) Length, out var value) ? value : string.Empty;
		}

		var str = MemoryMarshal.AsBytes(new Span<BlueString>(ref this))[..(int) Length];
		return str.ReadString(Encoding.UTF8) ?? string.Empty;
	}
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueFunctionSignature {
	public MemoryCStrPtr ReturnType { get; set; }
	public BlueArgumentTypes ArgumentTypes { get; set; }
	public uint ArgumentCount { get; set; }
	public uint OptionalCount { get; set; }
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueClassInfo {
	public MemoryPtr<BlueCLSID> ClassId { get; set; }
	public MemoryPtr<BlueIID> IID { get; set; }
	public MemoryCStrPtr Description { get; set; }
	public MemoryPtr<BlueInterfaceEntry> InterfaceTablePtr { get; set; }
	public MemoryPtr<BlueVarEntry> MemberTablePtr { get; set; }
	public MemoryPtr<BlueMethodDefinition> MethodTablePtr { get; set; }
	public MemoryPtr<BlueClassInfo> ParentClassInfo { get; set; }
	public nint OffsetToParent { get; set; }
	public nint Rtti { get; set; } // IBlueRtti* (ad-hoc instantiated)
	public nint TypeObject { get; set; } // PyTypeObject
	public uint LiveCount { get; set; } // CcpAtomic<uint32_t>
	public uint LockCount { get; set; } // CcpAtomic<uint32_t>
	public MemoryPtr<BlueMap<BlueString, BlueFunctionSignature>> FunctionSignaturesPtr { get; set; }

	public IEnumerable<BlueInterfaceEntry> InterfaceTable {
		get {
			var idx = 0;
			do {
				if (!InterfaceTablePtr.TryRead(idx++, out var iface) || iface.IID.IsNull) {
					yield break;
				}

				yield return iface;
			} while (true);
		}
	}

	public IEnumerable<BlueVarEntry> MemberTable {
		get {
			var idx = 0;
			do {
				if (!MemberTablePtr.TryRead(idx++, out var var) || var.Type == BlueTypeId.Invalid) {
					yield break;
				}

				yield return var;
			} while (true);
		}
	}

	public IEnumerable<BlueMethodDefinition> MethodTable {
		get {
			var idx = 0;
			do {
				if (!MethodTablePtr.TryRead(idx++, out var method) || method.Method == nint.Zero) {
					yield break;
				}

				yield return method;
			} while (true);
		}
	}
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueClassRegistration {
	public MemoryPtr<BlueClassInfo> Type { get; set; }
	public nint CreateFn { get; set; }
	public BlueClassRegistrationFlags Flags { get; set; }
}

// BlueVector, BlueMap, and BlueHashMap are wrappers around Windows STL std::vector, std::map, and std::hash_map

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueVector<T> where T : unmanaged {
	public MemoryPtr<T> First { get; set; }
	public MemoryPtr<T> Last { get; set; }
	public MemoryPtr<T> End { get; set; }

	public IEnumerable<T> Items() {
		if (First.IsNull || Last.IsNull || End.IsNull) {
			yield break;
		}

		var address = First;
		while (address.Address < Last.Address) {
			yield return address;
			address = new MemoryPtr<T>(address.Address + Unsafe.SizeOf<T>());
		}
	}
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueMapPair<TKey, TValue> where TKey : unmanaged where TValue : unmanaged {
	public TKey Key { get; set; }
	public TValue Value { get; set; }
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueMapTreeNode<TKey, TValue> where TKey : unmanaged where TValue : unmanaged {
	public MemoryPtr<BlueMapTreeNode<TKey, TValue>> Left { get; set; }
	public MemoryPtr<BlueMapTreeNode<TKey, TValue>> Parent { get; set; }
	public MemoryPtr<BlueMapTreeNode<TKey, TValue>> Right { get; set; }
	[field: MarshalAs(UnmanagedType.I1)] public byte Color { get; set; }
	[field: MarshalAs(UnmanagedType.I1)] public bool IsNull { get; set; }
	public BlueMapPair<TKey, TValue> Pair { get; set; }
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueMap<TKey, TValue> where TKey : unmanaged where TValue : unmanaged {
	public MemoryPtr<BlueMapTreeNode<TKey, TValue>> Head { get; set; }
	public nuint Count { get; set; }

	public IEnumerable<KeyValuePair<TKey, TValue>> Items() {
		if (Head.IsNull) {
			yield break;
		}

		var head = Head.Value;
		var currentPtr = head.Left;
		if (currentPtr.IsNull) {
			yield break;
		}

		while (currentPtr.Address != Head.Address) {
			var currentNode = currentPtr.Value;

			if (currentNode.IsNull) {
				break;
			}

			var pair = currentNode.Pair;
			yield return new KeyValuePair<TKey, TValue>(pair.Key, pair.Value);

			if (!currentNode.Right.Value.IsNull) {
				currentPtr = currentNode.Right;
				while (!currentPtr.Value.Left.Value.IsNull) {
					currentPtr = currentPtr.Value.Left;
				}
			} else {
				var parentPtr = currentNode.Parent;
				while (!parentPtr.IsNull && currentPtr.Address == parentPtr.Value.Right.Address) {
					currentPtr = parentPtr;
					parentPtr = parentPtr.Value.Parent;
				}

				currentPtr = parentPtr;
			}
		}
	}
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueHashMap<TKey, TValue> where TKey : unmanaged where TValue : unmanaged {
	public nuint Config { get; set; }
	public BlueTracked<BlueMap<TKey, TValue>> Map { get; set; }

	public IEnumerable<KeyValuePair<TKey, TValue>> Items() => Map.Value.Items();
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueTracked<T> where T : unmanaged {
	public BlueTrackerName Name { get; set; }
	public T Value { get; set; }
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BlueTypes {
	public BlueTracked<BlueVector<BlueGenericThunker>> GenericThunks { get; set; }
	public BlueTracked<BlueMap<MemoryPtr<BlueCLSID>, MemoryPtr<BlueClassRegistration>>> Classes { get; set; }
	public BlueHashMap<MemoryCStrPtr, MemoryPtr<BlueClassRegistration>> ClassesByName { get; set; }
}

#pragma warning restore CA1815
