using System.IO;
using System.Text;

namespace Jackdaw.Tests;

internal class StartParserTests {
	private const string SAMPLE = """
	                              [main]
	                              version = 21.03
	                              build = 2239025
	                              codename = EVE-RELEASE
	                              region = ccp
	                              cryptoPack = CryptoAPI
	                              sync = 2239025
	                              branch = //eve/branches/release/V21.03
	                              appname = EVE
	                              useScriptIndexFiles = 1
	                              socketIO = iocp
	                              server = Tranquility
	                              edition = premium
	                              role = client
	                              aid = 0
	                              resFromStuffOnly = 0
	                              port = 26000
	                              """;

	[Test]
	public void TestRealExample() {
		using var stream = new MemoryStream(Encoding.UTF8.GetBytes(SAMPLE));
		var result = StartParser.Parse(stream);

		Assert.Multiple(() => {
			Assert.That(result.Version, Is.EqualTo("21.03"));
			Assert.That(result.Build, Is.EqualTo("2239025"));
			Assert.That(result.Codename, Is.EqualTo("EVE-RELEASE"));
			Assert.That(result.Region, Is.EqualTo("ccp"));
			Assert.That(result.CryptoPack, Is.EqualTo("CryptoAPI"));
			Assert.That(result.Sync, Is.EqualTo("2239025"));
			Assert.That(result.Branch, Is.EqualTo("//eve/branches/release/V21.03"));
			Assert.That(result.AppName, Is.EqualTo("EVE"));
			Assert.That(result.UseScriptIndexFiles, Is.EqualTo(true));
			Assert.That(result.SocketIO, Is.EqualTo("iocp"));
			Assert.That(result.Server, Is.EqualTo("Tranquility"));
			Assert.That(result.Edition, Is.EqualTo("premium"));
			Assert.That(result.Role, Is.EqualTo("client"));
			Assert.That(result.Aid, Is.EqualTo(false));
			Assert.That(result.ResFromStuffOnly, Is.EqualTo(false));
			Assert.That(result.Port, Is.EqualTo(26000));
		});
	}
}
