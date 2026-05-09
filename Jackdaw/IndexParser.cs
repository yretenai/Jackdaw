using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Jackdaw.Structs.Client;
using Pluto.IO.Binary;

namespace Jackdaw;

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

	public static unsafe ResourceCacheRecord[] Parse(RentedArray<byte> data) {
		var span = data.Span;
		fixed (byte* ptr = &span.GetPinnableReference()) {
			using var stream = new UnmanagedMemoryStream(ptr, data.Length);
			return Parse(stream);
		}
	}
}
