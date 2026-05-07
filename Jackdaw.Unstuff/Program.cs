using System.Diagnostics;
using Pluto.CommandLine;
using Jackdaw.Stuff;
using Pluto.IO.FileSystem;

namespace Jackdaw.Unstuff;

internal static class Program {
	public static UnstuffFlags Flags { get; set; } = null!;

	private static void Main(string[] args) {
		var flags = CommandLineFlagsParser.ParseFlags<UnstuffFlags>();
		Flags = flags;

		var options = new EnumerationOptions {
			MatchCasing = MatchCasing.CaseInsensitive,
			MatchType = MatchType.Simple,
			RecurseSubdirectories = true,
		};

		foreach (var stuff in new FileEnumerator(Flags.Resource, options, "*.stuff")) {
			Extract(stuff);
		}
	}

	private static void Extract(string stuffPath) {
		using var stream = new FileStream(stuffPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		using var stuff = new EmbedFS(stream);

		foreach (var file in stuff.Names) {
			using var buffer = stuff.Open(file);
			if (buffer == null) {
				continue;
			}

			var safeName = file;
			if (safeName.StartsWith("res:") || safeName.StartsWith("res/")) {
				safeName = safeName[4..];
			}

			safeName = safeName.TrimStart('.', '/', '~').ToLowerInvariant();
			Trace.Assert(!safeName.Contains(':', StringComparison.Ordinal));

			Console.WriteLine($"{Path.GetFileNameWithoutExtension(stuffPath)}: {safeName}");
			if (Flags.Dry) {
				continue;
			}

			var dir = Path.GetDirectoryName(safeName);
			if (!string.IsNullOrEmpty(dir)) {
				Directory.CreateDirectory(Path.Combine(Flags.Output, dir));
			}

			using var output = new FileStream(Path.Combine(Flags.Output, safeName), FileMode.Create, FileAccess.Write);
			output.Write(buffer.Span);
		}
	}
}
