namespace MEFL.Contract;

public class LauncherWebVersionInfo
{
	public string Id { get; set; }

	public string Url { get; set; }

	public string Type { get; set; }
	public string Time { get; set; }
	public override string ToString()
	{
		return $"{Id},{Type}";
	}
}
