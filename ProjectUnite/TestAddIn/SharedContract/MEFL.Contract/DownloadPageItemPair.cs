using System.Collections.Generic;

namespace MEFL.Contract;

public class DownloadPageItemPair
{
	public bool HasError { get; set; }
	public bool IsRefreshing { get; set; }
	public delegate void delRefreshEvent(object sender, string tmpFolderPath);

	public string Title { get; private set; }

	public string Tag { get; set; }
	public string ErrorDescription;

	public List<LauncherWebVersionInfo> Content { get; set; }

	public event delRefreshEvent? RefreshEvent;
	public void Refresh(string tmpFolderPath)
	{
		RefreshEvent?.Invoke(this, tmpFolderPath);
	}

	public DownloadPageItemPair(string title, List<LauncherWebVersionInfo> items, string tag)
	{
		Title = title;
		Content = items;
		Tag = tag;
	}
}
