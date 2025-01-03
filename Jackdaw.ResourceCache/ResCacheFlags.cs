using System.Collections.Generic;
using DragonLib.CommandLine;

namespace Jackdaw.ResourceCache;

public record ResCacheFlags : CommandLineFlags {
	[Flag("res-cache", Positional = 0, IsRequired = true, Help = "Path to the Resource Cache")]
	public string ResCache { get; set; } = null!;

	[Flag("output", Positional = 1, IsRequired = true, Help = "Path to where files should be actualized")]
	public string Output { get; set; } = null!;

	[Flag("res-cache", Positional = 2, IsRequired = true, Help = "Path to different resource indexes")]
	public List<string> IndexFiles { get; set; } = [];

	[Flag("dry", Help = "Dry run, don't actually download/create anything")]
	public bool Dry { get; set; }

	[Flag("symlink", Help = "Enforce symbolic link creation")]
	public bool Symlink { get; set; }

	[Flag("no-symlink", Help = "Prevent symbolic link creation")]
	public bool NoSymlink { get; set; }

	[Flag("no-download", Help = "Don't download any resources")]
	public bool NoDownload { get; set; }

	[Flag("no-overwrite", Help = "Don't overwrite any resources")]
	public bool NoOverwrite { get; set; }
}
