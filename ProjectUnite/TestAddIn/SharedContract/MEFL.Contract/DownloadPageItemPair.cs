using System.Collections.Generic;

namespace MEFL.Contract;

public class LauncherWebVersionInfoList: List<LauncherWebVersionInfo>
{
	public string VersionMajor;
	public LauncherWebVersionInfoList(string versionMajor)
	{
		VersionMajor = versionMajor;
	}

	public override string ToString()
	{
		return VersionMajor;
	}
}

public class DownloadPageItemPair
{
	public bool HasError { get; set; }
	public bool IsRefreshing { get; set; }
	public delegate void delRefreshEvent(object sender, string tmpFolderPath);

	public string Title { get; private set; }

	public string Tag { get; set; }
	public string ErrorDescription;

    public List<LauncherWebVersionInfoList> Contents { get; set; }

	public event delRefreshEvent? RefreshEvent;
	public void Refresh(string tmpFolderPath)
	{
		RefreshEvent?.Invoke(this, tmpFolderPath);
	}

	public DownloadPageItemPair(string title, List<LauncherWebVersionInfoList> contents, string tag)
	{
		Title = title;
		Contents = contents;
		Tag = tag;
	}
}
