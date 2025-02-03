using System.Collections.Generic;
using DragonLib.CommandLine;

namespace Jackdaw.ResourceCache;

public record ResCacheBasicFlags : CommandLineFlags {
	[Flag("res-cache", Positional = 0, IsRequired = true, Help = "Path to the Resource Cache")]
	public string ResCache { get; set; } = null!;

	[Flag("repair", Help = "Repair")]
	public bool Repair { get; set; }

	[Flag("no-download", Help = "Don't download any resources")]
	public bool NoDownload { get; set; }

	[Flag("dry", Help = "Dry run, don't actually download/create anything")]
	public bool Dry { get; set; }
}

public record ResCacheFlags : ResCacheBasicFlags {
	[Flag("output", Positional = 1, IsRequired = true, Help = "Path to where files should be actualized")]
	public string Output { get; set; } = null!;

	[Flag("res-cache", Positional = 2, IsRequired = true, Help = "Path to different resource indexes, or path to flycatcher cache followed by a version number")]
	public List<string> IndexFiles { get; set; } = [];

	[Flag("symlink", Help = "Enforce symbolic link creation")]
	public bool Symlink { get; set; }

	[Flag("no-symlink", Help = "Prevent symbolic link creation")]
	public bool NoSymlink { get; set; }

	[Flag("no-overwrite", Help = "Don't overwrite any resources")]
	public bool NoOverwrite { get; set; }

	[Flag("no-deduplication", Help = "Don't deduplicate files with the same hash")]
	public bool NoDeduplication { get; set; }

	[Flag("clean", Help = "Clean entire tree when updating")]
	public bool Clean { get; set; }

	[Flag("update", Help = "Only update changed files")]
	public bool Update { get; set; }

	[Flag("reclaim", Help = "Remove unused resources files from the cache")]
	public bool Reclaim { get; set; }
}
