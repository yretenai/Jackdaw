namespace Knit;

public sealed class Granny2File : IDisposable, IAsyncDisposable {
	public Granny2File(Stream stream) => BaseStream = stream;

	// note: we probably are going to reconstruct the whole stream with decompression, so don't store BaseStream?
	public Stream BaseStream { get; }

	public async ValueTask DisposeAsync() {
		await BaseStream.DisposeAsync();
	}

	public void Dispose() {
		BaseStream.Dispose();
	}
}
