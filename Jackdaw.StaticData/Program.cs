using CommunityToolkit.HighPerformance.Buffers;
using Ferment;
using Jackdaw.FSD;
using Jackdaw.StaticData.Converters;
using Jackdaw.Trinity;
using Newtonsoft.Json;
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
		var erroredFiles = new Dictionary<string, HashSet<string>>();

		switch (mode) {
			case "black":
				ProcessBlack(files, erroredFiles);
				break;
			case "fsdbinary":
				ProcessFSD(files, erroredFiles);
				break;
			case "pickle":
				ProcessPickle(files, erroredFiles);
				break;
		}

		if (erroredFiles.Count > 0) {
			Log.Error("Failed to process {Count} files", erroredFiles.Count);
			foreach (var (exception, erroredFileSet) in erroredFiles) {
				Log.Error("Exception: {Exception}", exception);
				foreach (var file in erroredFileSet) {
					Log.Error("\t{File}", file);
				}
			}
		}
	}

	private static void ProcessPickle(string[] files, Dictionary<string, HashSet<string>> erroredFiles) {
		var current = 0;
		foreach (var file in files) {
			try {
				Log.Information("[{Current}/{Total}] Processing {File}", ++current, files.Length, file);
				using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				using var pickle = new Unpickler(fs);
				var data = pickle.Read();
				File.WriteAllText(Path.ChangeExtension(file, ".json"), JsonConvert.SerializeObject(data, Formatting.Indented));
			} catch (Exception ex) {
				Log.Error(ex, "Failed to process {File}", file);
				if (!erroredFiles.TryGetValue(ex.Message, out var erroredFilesForException)) {
					erroredFilesForException = [];
					erroredFiles.Add(ex.Message, erroredFilesForException);
				}

				erroredFilesForException.Add(file);
			}
		}
	}

	private static void ProcessFSD(string[] files, Dictionary<string, HashSet<string>> erroredFiles) {
		var current = 0;
		foreach (var file in files) {
			try {
				Log.Information("[{Current}/{Total}] Processing {File}", ++current, files.Length, file);
				using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				using var owner = MemoryOwner<byte>.Allocate((int) fs.Length);
				fs.ReadExactly(owner.Memory.Span);
				var reader = new FSDBinary(owner);
				File.WriteAllText(Path.ChangeExtension(file, ".json"), JsonConvert.SerializeObject(reader.Value, Formatting.Indented, new FSDColorConverter(), new FSDResourceConverter(), new FSDStringConverter()));
			} catch (Exception ex) {
				Log.Error(ex, "Failed to process {File}", file);
				if (!erroredFiles.TryGetValue(ex.Message, out var erroredFilesForException)) {
					erroredFilesForException = [];
					erroredFiles.Add(ex.Message, erroredFilesForException);
				}

				erroredFilesForException.Add(file);
			}
		}
	}

	private static void ProcessBlack(string[] files, Dictionary<string, HashSet<string>> erroredFiles) {
		var current = 0;
		foreach (var file in files) {
			try {
				Log.Information("[{Current}/{Total}] Processing {File}", ++current, files.Length, file);
				using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				using var owner = MemoryOwner<byte>.Allocate((int) fs.Length);
				fs.ReadExactly(owner.Memory.Span);
				var reader = new BlackFile(owner);
				File.WriteAllText(Path.ChangeExtension(file, ".json"), JsonConvert.SerializeObject(reader.Root, Formatting.Indented, new TriFloatConverter(), new EveSOFDataGenericStringConverter()));
			} catch (Exception ex) {
				Log.Error(ex, "Failed to process {File}", file);
				if (!erroredFiles.TryGetValue(ex.Message, out var erroredFilesForException)) {
					erroredFilesForException = [];
					erroredFiles.Add(ex.Message, erroredFilesForException);
				}

				erroredFilesForException.Add(file);
			}
		}
	}
}
