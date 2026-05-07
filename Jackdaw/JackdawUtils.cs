using System;
using System.IO;
using Charon.Compression;
using Pluto.IO.Binary;

namespace Jackdaw;

public static class JackdawUtils {
	public static RentedArray<byte> Decompress(Stream stream) {
		using var rented = new RentedArray<byte>((int) stream.Length);
		stream.ReadExactly(rented.Span);
		return Decompress(rented.Memory);
	}

	public static RentedArray<byte> Decompress(Memory<byte> data) {
		var size = ZStandard.GetDecompressBound(data);
		var rented = new RentedArray<byte>(size);
		CompressionHelper.Decompress(CompressionType.Zstd, data, rented.Memory);
		return rented;
	}

	public static RentedArray<byte> DecompressGz(Stream stream) {
		using var rented = new RentedArray<byte>((int) stream.Length);
		stream.ReadExactly(rented.Span);
		return DecompressGz(rented.Memory);
	}

	public static RentedArray<byte> DecompressGz(Memory<byte> data) {
		var rented = new RentedArray<byte>(data.Length * 16);
		var n = CompressionHelper.Decompress(CompressionType.GzipUnknownSize, data, rented.Memory);
		rented.Length = n;
		return rented;
	}

	public static RentedArray<byte> Compress(Stream stream) {
		using var rented = new RentedArray<byte>((int) stream.Length);
		stream.ReadExactly(rented.Span);
		return Compress(rented.Memory);
	}

	public static RentedArray<byte> Compress(Memory<byte> data) {
		var rented = new RentedArray<byte>(data.Length);
		var n = CompressionHelper.Compress(CompressionType.Zstd, data, rented.Memory);
		rented.Length = n;
		return rented;
	}

	public static RentedArray<byte> CompressGz(Stream stream) {
		using var rented = new RentedArray<byte>((int) stream.Length);
		stream.ReadExactly(rented.Span);
		return CompressGz(rented.Memory);
	}

	public static RentedArray<byte> CompressGz(Memory<byte> data) {
		var rented = new RentedArray<byte>(data.Length);
		var n = CompressionHelper.Compress(CompressionType.Gzip, data, rented.Memory);
		rented.Length = n;
		return rented;
	}
}
