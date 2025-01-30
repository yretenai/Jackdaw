using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jackdaw.Stuff;

public sealed class EmbedFS : IDisposable, IAsyncDisposable {
	public EmbedFS(Stream stream) {
		BaseStream = stream;

		Span<byte> slop = stackalloc byte[0xFFF];
		BaseStream.Seek(-0x10, SeekOrigin.End);
		BaseStream.ReadExactly(slop[..0x10]);
		if (!slop[..0x10].SequenceEqual("\0\0\0\0EmbedFs 1.0\0"u8)) {
			throw new InvalidDataException("Not an EmbedFS 1.0 file");
		}

		BaseStream.Position = 0;

		var numberOfResources = 0;
		BaseStream.ReadExactly(MemoryMarshal.AsBytes(new Span<int>(ref numberOfResources)));
		Resources.EnsureCapacity(numberOfResources);

		Span<int> info = stackalloc int[2];
		var buffer = MemoryMarshal.AsBytes(info);

		var localOffset = 0L;
		for (var index = 0; index < numberOfResources; ++index) {
			BaseStream.ReadExactly(buffer);

			var size = info[0];
			var nameLength = info[1];
			var name = slop[..nameLength];

			BaseStream.ReadExactly(name);
			BaseStream.Position += 1; // skip null byte terminator

			Resources[Encoding.UTF8.GetString(name).Replace('\\', '/')] = (localOffset, size);
			localOffset += size;
		}

		BaseAddress = BaseStream.Position;
	}

	public Stream BaseStream { get; }
	public long BaseAddress { get; }
	public Dictionary<string, (long Offset, int Size)> Resources { get; } = new(StringComparer.OrdinalIgnoreCase);
	public IEnumerable<string> Names => Resources.Keys;

	public async ValueTask DisposeAsync() {
		await BaseStream.DisposeAsync();
	}

	public void Dispose() {
		BaseStream.Dispose();
	}

	public IMemoryOwner<byte>? Open(string name, out int allocSize) {
		allocSize = 0;
		if (!Resources.TryGetValue(name.Replace('\\', '/'), out var info)) {
			return null;
		}

		BaseStream.Seek(BaseAddress + info.Offset, SeekOrigin.Begin);
		var buffer = MemoryPool<byte>.Shared.Rent(info.Size);
		try {
			BaseStream.ReadExactly(buffer.Memory.Span[..info.Size]);
			allocSize = info.Size;
			return buffer;
		} catch {
			buffer.Dispose();
			throw;
		}
	}
}
