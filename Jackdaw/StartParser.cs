using System;
using System.IO;
using IronCompress;
using Jackdaw.Structs.Client;

namespace Jackdaw;

public static class StartParser {
	public static StartInfo Parse(string path) {
		using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		return Parse(fs);
	}

	public static StartInfo Parse(Stream stream, bool leaveOpen = false) {
		using var reader = new StreamReader(stream, leaveOpen: leaveOpen);
		var info = new StartInfo();

		while (!reader.EndOfStream) {
			var line = reader.ReadLine();
			if (line == null) {
				break;
			}

			line = line.Trim();
			var parts = line.Split('=', 2, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length != 2) {
				continue;
			}

			if (parts[0].Equals(nameof(StartInfo.Version), StringComparison.OrdinalIgnoreCase)) {
				info.Version = parts[1];
			} else if (parts[0].Equals(nameof(StartInfo.Build), StringComparison.OrdinalIgnoreCase)) {
				info.Build = parts[1];
			} else if (parts[0].Equals(nameof(StartInfo.Codename), StringComparison.OrdinalIgnoreCase)) {
				info.Codename = parts[1];
			} else if (parts[0].Equals(nameof(StartInfo.Region), StringComparison.OrdinalIgnoreCase)) {
				info.Region = parts[1];
			} else if (parts[0].Equals(nameof(StartInfo.CryptoPack), StringComparison.OrdinalIgnoreCase)) {
				info.CryptoPack = parts[1];
			} else if (parts[0].Equals(nameof(StartInfo.Sync), StringComparison.OrdinalIgnoreCase)) {
				info.Sync = parts[1];
			} else if (parts[0].Equals(nameof(StartInfo.Branch), StringComparison.OrdinalIgnoreCase)) {
				info.Branch = parts[1];
			} else if (parts[0].Equals(nameof(StartInfo.AppName), StringComparison.OrdinalIgnoreCase)) {
				info.AppName = parts[1];
			} else if (parts[0].Equals(nameof(StartInfo.UseScriptIndexFiles), StringComparison.OrdinalIgnoreCase)) {
				info.UseScriptIndexFiles = parts[1] == "1";
			} else if (parts[0].Equals(nameof(StartInfo.SocketIO), StringComparison.OrdinalIgnoreCase)) {
				info.SocketIO = parts[1];
			} else if (parts[0].Equals(nameof(StartInfo.Server), StringComparison.OrdinalIgnoreCase)) {
				info.Server = parts[1];
			} else if (parts[0].Equals(nameof(StartInfo.Edition), StringComparison.OrdinalIgnoreCase)) {
				info.Edition = parts[1];
			} else if (parts[0].Equals(nameof(StartInfo.Role), StringComparison.OrdinalIgnoreCase)) {
				info.Role = parts[1];
			} else if (parts[0].Equals(nameof(StartInfo.Aid), StringComparison.OrdinalIgnoreCase)) {
				info.Aid = parts[1] == "1";
			} else if (parts[0].Equals(nameof(StartInfo.ResFromStuffOnly), StringComparison.OrdinalIgnoreCase)) {
				info.ResFromStuffOnly = parts[1] == "1";
			} else if (parts[0].Equals(nameof(StartInfo.Port), StringComparison.OrdinalIgnoreCase)) {
				info.Port = int.Parse(parts[1]);
			}
		}

		return info;
	}

	public static unsafe StartInfo Parse(IronCompressResult data) {
		var span = data.AsSpan();
		fixed (byte* ptr = &span.GetPinnableReference()) {
			using var stream = new UnmanagedMemoryStream(ptr, data.Length);
			return Parse(stream);
		}
	}
}
