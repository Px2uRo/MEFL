using HarfBuzzSharp;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MEFL.Contract;

public class LauncherWebVersionInfoList: ObservableCollection<LauncherWebVersionInfo>
{
	public virtual new void RemoveItem(int index)
	{
		this[index].Dispose();
		base.RemoveItem(index);	
	}
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

public abstract class DownloadPageItemPair:MEFLClass
{
	protected override void Dispose(bool disposing)
	{
		for (int i = 0; i < Contents.Count;)
		{
			Contents[i].Clear();
			Contents.RemoveAt(i);
		}
		base.Dispose(disposing);
	}
	public bool HasError { get; set; }
	public delegate void RefreshEvent(object sender, string tmpFolderPath);

	public string Title { get; protected set; }

	public string Tag { get; set; }
	public string ErrorDescription;

    public List<LauncherWebVersionInfoList> Contents { get; set; }

    public event RefreshEvent? ListRefreshEvent;
    public void RefreshList(string tmpFolderPath)
    {
        ListRefreshEvent?.Invoke(this, tmpFolderPath);
    }
    public event RefreshEvent? WebRefreshEvent;
    public virtual void WebRefresh(string tmpFolderPath)
    {
        WebRefreshEvent?.Invoke(this, tmpFolderPath);
    }
	public DownloadPageItemPair(string title, List<LauncherWebVersionInfoList> contents, string tag)
	{
        Title = title;
        Contents = contents;
        Tag = tag;
    }
    public virtual void RefreshCompete()
    {
		RefreshCompeted?.Invoke(this,EventArgs.Empty);
    }

	public event EventHandler RefreshCompeted;
}
