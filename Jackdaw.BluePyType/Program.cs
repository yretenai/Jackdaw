using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using Jackdaw.BluePyType.MemoryHandlers;

namespace Jackdaw.BluePyType;

internal class Program {
	private static JsonSerializerOptions Options { get; } = new() {
		WriteIndented = true,
		Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
	};

	private static void Main(string[] args) {
		if (args.Length < 1) {
			Console.WriteLine("Usage: Jackdaw.BluePyType <pid or memdump path>");
			return;
		}

		var pidOrFile = args[0];
		IMemoryHandler handler;
		if (int.TryParse(pidOrFile, NumberStyles.Integer, null, out var pid)) {
			if (OperatingSystem.IsLinux()) {
				handler = new IOVHandler(pid);
			} else if (OperatingSystem.IsWindows()) {
				handler = new MemoryApiHandler(pid);
			} else {
				throw new InvalidOperationException($"{RuntimeInformation.OSDescription} is not supported");
			}
		} else {
			handler = new MinidumpHandler(pidOrFile);
		}

		IMemoryHandler.Handler = handler;

		try {
			var beClasses = new BeClasses();
			using var stream = new FileStream("BluePyType.json", FileMode.Create, FileAccess.ReadWrite);
			JsonSerializer.Serialize(stream, beClasses.Classes, Options);
			Console.WriteLine($"Saved {beClasses.Classes.Count} classes");
		} finally {
			handler.Dispose();
		}
	}
}
