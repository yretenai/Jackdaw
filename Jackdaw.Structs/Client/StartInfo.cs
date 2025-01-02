namespace Jackdaw.Structs.Client;

public record StartInfo {
	public string Version { get; set; } = "0.0";
	public string Build { get; set; } = "0";
	public string Codename { get; set; } = "";
	public string Region { get; set; } = "";
	public string CryptoPack { get; set; } = "";
	public string Sync { get; set; } = "0";
	public string Branch { get; set; } = "";
	public string AppName { get; set; } = "";
	public bool UseScriptIndexFiles { get; set; } = true;
	public string SocketIO { get; set; } = "";
	public string Server { get; set; } = "";
	public string Edition { get; set; } = "";
	public string Role { get; set; } = "";
	public bool Aid { get; set; }
	public bool ResFromStuffOnly { get; set; }
	public int Port { get; set; } = 26000;
}
