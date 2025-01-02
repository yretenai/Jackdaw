using System.IO;
using System.Text;
using Jackdaw.Cache;

namespace Jackdaw.Tests.Cache;

internal class IndexParserTests {
	private const string SAMPLE = """
	                              res:/intromovie.txt,a9/a9d1721dd5cc6d54_e6bbb2df307e5a9527159a4c971034b5,e6bbb2df307e5a9527159a4c971034b5,9719,3312
	                              res:/videocardcategories.yaml,ab/ab5cde4fbbf82fb6_6a9d6d4c6015616877b77865209c5064,6a9d6d4c6015616877b77865209c5064,33591,5076
	                              """;

	[Test]
	public void ParseIndex() {
		using var stream = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));
		var result = IndexParser.Parse(stream);

		Assert.That(result, Has.Length.EqualTo(2));

		Assert.Multiple(() => {
			Assert.That(result[0].Path.OriginalString, Is.EqualTo("res:/intromovie.txt"));
			Assert.That(result[0].ResourcePath, Is.EqualTo("a9/a9d1721dd5cc6d54_e6bbb2df307e5a9527159a4c971034b5"));
			Assert.That(result[0].MD5, Is.EqualTo("e6bbb2df307e5a9527159a4c971034b5"));
			Assert.That(result[0].Size, Is.EqualTo(9719));
			Assert.That(result[0].CompressedSize, Is.EqualTo(3312));

			Assert.That(result[1].Path.OriginalString, Is.EqualTo("res:/videocardcategories.yaml"));
			Assert.That(result[1].ResourcePath, Is.EqualTo("ab/ab5cde4fbbf82fb6_6a9d6d4c6015616877b77865209c5064"));
			Assert.That(result[1].MD5, Is.EqualTo("6a9d6d4c6015616877b77865209c5064"));
			Assert.That(result[1].Size, Is.EqualTo(33591));
			Assert.That(result[1].CompressedSize, Is.EqualTo(5076));
		});
	}
}
