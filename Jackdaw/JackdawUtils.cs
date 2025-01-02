using System;
using System.IO;
using System.IO.Compression;
using CommunityToolkit.HighPerformance.Buffers;
using IronCompress;

namespace Jackdaw;

public static class JackdawUtils {
	public static Iron Iron { get; } = new();

	public static IronCompressResult Decompress(Stream stream) {
		using var rented = MemoryOwner<byte>.Allocate((int) stream.Length);
		stream.ReadExactly(rented.Memory.Span);
		return Decompress(rented.Memory.Span);
	}

	public static IronCompressResult Decompress(Span<byte> data) {
		var decompressed = Iron.Decompress(Codec.Zstd, data);
		return decompressed;
	}

	public static IronCompressResult DecompressGz(Stream stream) {
		using var rented = MemoryOwner<byte>.Allocate((int) stream.Length);
		stream.ReadExactly(rented.Memory.Span);
		return DecompressGz(rented.Memory.Span);
	}

	public static IronCompressResult DecompressGz(Span<byte> data) {
		var decompressed = Iron.Decompress(Codec.Gzip, data);
		return decompressed;
	}

	public static IronCompressResult Compress(Stream stream) {
		using var rented = MemoryOwner<byte>.Allocate((int) stream.Length);
		stream.ReadExactly(rented.Memory.Span);
		return Compress(rented.Memory.Span);
	}

	public static IronCompressResult Compress(Span<byte> data) {
		var decompressed = Iron.Compress(Codec.Zstd, data, null, CompressionLevel.SmallestSize);
		return decompressed;
	}

	public static IronCompressResult CompressGz(Stream stream) {
		using var rented = MemoryOwner<byte>.Allocate((int) stream.Length);
		stream.ReadExactly(rented.Memory.Span);
		return CompressGz(rented.Memory.Span);
	}

	public static IronCompressResult CompressGz(Span<byte> data) {
		var decompressed = Iron.Compress(Codec.Gzip, data, null, CompressionLevel.SmallestSize);
		return decompressed;
	}
}
