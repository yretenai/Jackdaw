using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using Ferment;
using Jackdaw.FSD;
using Jackdaw.StaticData.Converters;
using Jackdaw.Trinity;
using Serilog;

namespace Jackdaw.StaticData;

internal class Program {
	private static void Main(string[] args) {
		if (args.Length < 2) {
			Console.WriteLine("Usage: Jackdaw.StaticData [black/fsdbinary/pickle] <path to file, filelist or directory...>");
			return;
		}

		Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Console().CreateLogger();

		var mode = args[0].ToLower();
		if (mode is not ("black" or "fsdbinary" or "pickle")) {
			Log.Error("{Mode} is not a supported mode.", mode);
			return;
		}

		var files = args.Skip(1).SelectMany(arg => Directory.Exists(arg) ? Directory.EnumerateFiles(arg, $"*.{mode}", SearchOption.AllDirectories) : Path.GetExtension(arg).Equals(".txt", StringComparison.Ordinal) ? File.ReadAllLines(arg) : [arg]).Order().ToArray();

		switch (mode) {
			case "black":
				ProcessBlack(files);
				break;
			case "fsdbinary":
				ProcessFSD(files);
				break;
			case "pickle":
				ProcessPickle(files);
				break;
		}
	}

	private static readonly JsonSerializerOptions JsonOptions = new() {
		WriteIndented = true,
		Converters = {
			new FSDColorConverter(),
			new FSDResourceConverter(),
			new FSDStringConverter(),
			new EveSOFDataGenericStringConverter(),
			new TriFloatConverter(),
			new PolymorphicConverterFactory(),
		},
		NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
		ReferenceHandler = ReferenceHandler.Preserve,
		Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
	};

	private static void ProcessPickle(string[] files) {
		var current = 0;
		foreach (var file in files) {
			try {
				Log.Information("[{Current}/{Total}] Processing {File}", ++current, files.Length, file);
				using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				using var pickle = new Unpickler(fs);
				var data = pickle.Read();
				using var stream = new FileStream(Path.ChangeExtension(file, ".json"), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
				JsonSerializer.Serialize(stream, data, JsonOptions);
			} catch (Exception ex) {
				Log.Error(ex, "Failed to process {File}", file);
			}
		}
	}

	private static void ProcessFSD(string[] files) {
		var current = 0;
		foreach (var file in files) {
			try {
				Log.Information("[{Current}/{Total}] Processing {File}", ++current, files.Length, file);
				using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				using var owner = MemoryOwner<byte>.Allocate((int) fs.Length);
				fs.ReadExactly(owner.Memory.Span);
				var fsd = new FSDBinary(owner);
				using var stream = new FileStream(Path.ChangeExtension(file, ".json"), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
				JsonSerializer.Serialize(stream, fsd.Value, JsonOptions);
			} catch (Exception ex) {
				Log.Error(ex, "Failed to process {File}", file);
			}
		}
	}

	private static void ProcessBlack(string[] files) {
		var current = 0;
		foreach (var file in files) {
			try {
				Log.Information("[{Current}/{Total}] Processing {File}", ++current, files.Length, file);
				using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				using var owner = MemoryOwner<byte>.Allocate((int) fs.Length);
				fs.ReadExactly(owner.Memory.Span);
				var black = new BlackFile(owner);
				using var stream = new FileStream(Path.ChangeExtension(file, ".json"), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
				JsonSerializer.Serialize(stream, black.Root, JsonOptions);
			} catch (Exception ex) {
				Log.Error(ex, "Failed to process {File}", file);
			}
		}
	}
}
