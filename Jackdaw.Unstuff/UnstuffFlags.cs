using DragonLib.CommandLine;

namespace Jackdaw.Unstuff;

public record UnstuffFlags : CommandLineFlags {
	[Flag("res", Positional = 0, IsRequired = true, Help = "Path to the EmbedFs stuff archives (or directory)")]
	public string Resource { get; set; } = null!;

	[Flag("output", Positional = 1, IsRequired = true, Help = "Path to where files should be actualized")]
	public string Output { get; set; } = null!;

	[Flag("dry", Help = "Dry run, don't actually create anything")]
	public bool Dry { get; set; }
}
