using System;
using System.Buffers;
using System.IO;
using System.IO.Compression;
using IronCompress;

namespace Jackdaw;

public static class JackdawUtils {
	public static Iron Iron { get; } = new();

	public static IronCompressResult Decompress(Stream stream) {
		var size = (int) stream.Length;
		using var rented = MemoryPool<byte>.Shared.Rent(size);
		var block = rented.Memory.Span[..size];
		stream.ReadExactly(block);
		return Decompress(block);
	}

	public static IronCompressResult Decompress(Span<byte> data) {
		var decompressed = Iron.Decompress(Codec.Zstd, data);
		return decompressed;
	}

	public static IronCompressResult DecompressGz(Stream stream) {
		var size = (int) stream.Length;
		using var rented = MemoryPool<byte>.Shared.Rent(size);
		var block = rented.Memory.Span[..size];
		stream.ReadExactly(block);
		return DecompressGz(block);
	}

	public static IronCompressResult DecompressGz(Span<byte> data) {
		var decompressed = Iron.Decompress(Codec.Gzip, data);
		return decompressed;
	}

	public static IronCompressResult Compress(Stream stream) {
		var size = (int) stream.Length;
		using var rented = MemoryPool<byte>.Shared.Rent(size);
		var block = rented.Memory.Span[..size];
		stream.ReadExactly(block);
		return Compress(block);
	}

	public static IronCompressResult Compress(Span<byte> data) {
		var decompressed = Iron.Compress(Codec.Zstd, data, null, CompressionLevel.SmallestSize);
		return decompressed;
	}

	public static IronCompressResult CompressGz(Stream stream) {
		var size = (int) stream.Length;
		using var rented = MemoryPool<byte>.Shared.Rent(size);
		var block = rented.Memory.Span[..size];
		stream.ReadExactly(block);
		return CompressGz(block);
	}

	public static IronCompressResult CompressGz(Span<byte> data) {
		var decompressed = Iron.Compress(Codec.Gzip, data, null, CompressionLevel.SmallestSize);
		return decompressed;
	}
}
