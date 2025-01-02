using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using IronCompress;
using Jackdaw.Structs.Client;

namespace Jackdaw.Cache;

public static class IndexParser {
	public static ResourceCacheRecord[] Parse(string path) {
		using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		return Parse(fs);
	}

	public static ResourceCacheRecord[] Parse(Stream stream, bool leaveOpen = false) {
		using var reader = new StreamReader(stream, leaveOpen: leaveOpen);
		using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) {
			HasHeaderRecord = false,
		});
		return csv.GetRecords<ResourceCacheRecord>().ToArray();
	}

	public static unsafe ResourceCacheRecord[] Parse(IronCompressResult data) {
		var span = data.AsSpan();
		fixed (byte* ptr = &span.GetPinnableReference()) {
			using var stream = new UnmanagedMemoryStream(ptr, data.Length);
			return Parse(stream);
		}
	}
}
