using System.Collections.Generic;

namespace MEFL.Contract;

public class DownloadPageItemPair
{
	public delegate void RefreshBeihvior(object sender, string tmpFolderPath);

	public string Title { get; private set; }

	public string Tag { get; set; }

	public List<LauncherWebVersionInfo> Items { get; set; }

	public event RefreshBeihvior? RefreshEvent;

	public DownloadPageItemPair(string title, List<LauncherWebVersionInfo> items, string tag)
	{
		Title = title;
		Items = items;
		Tag = tag;
	}

	public void InvokeRefreshEvent(object sender, string tmpFolderPath)
	{
		this.RefreshEvent!(sender, tmpFolderPath);
	}
}
