using MEFL.Arguments;
using System.IO;

namespace MEFL.Contract;

public abstract class LauncherWebVersionInfo:MEFLClass
{
	public string Id { get; set; }

	public string Url { get; set; }

	public string Type { get; set; }
	public string Time { get; set; }
	public override string ToString()
	{
		return $"{Id},{Type}";
	}

	public abstract void Download(MEFLDownloader downloader, SettingArgs args);
}
