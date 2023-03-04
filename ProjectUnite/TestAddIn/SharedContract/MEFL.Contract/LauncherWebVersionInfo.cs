using MEFL.Arguments;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MEFL.Contract;

public abstract class LauncherWebVersionInfo:MEFLClass
{
	public string Id { get; set; }

	public string Url { get; set; }

	public string Type { get; set; }
	public string ReleaseTime { get; set; }
	public object Icon { get; set; }
	public override string ToString()
	{
		return $"{Id},{Type}";
	}

	public abstract InstallProgressInput Download(MEFLDownloader downloader,string dotMCFolder, SettingArgs args, DownloadSource[] sources);
}
/// <summary>
/// 启动器安装结果
/// </summary>
public class InstallProgressInput : MEFLClass
{
	public delegate void DownloadButtonClicked(InstallProgressInput result);
	public event DownloadButtonClicked DownloadButtonClickEvent;

    public void NowDownload()
	{
		if(DownloadButtonClickEvent != null)
		{
			DownloadButtonClickEvent(this);
		}
	}
	private bool _hasError;
	public bool HasError { get => _hasError; }
	private UserControl _page;

	public UserControl Page
	{
		get { return _page; }
	}
	private DownloadProgress? _progress;

	public DownloadProgress Progress
	{
		get { return _progress; }
		set { _progress = value; }
	}
	/// <summary>
	/// 安装进程结果
	/// </summary>
	/// <param name="hasError">是否有错误，如果是没有的话，progress 不要设为 null，有的话可以为 null</param>
	/// <param name="page">页面，MEFL 会自动将这个类作为这个页面的 DataContext 调用 NowDownload() 即可退出页面</param>
	/// <param name="progress">进程</param>
	public InstallProgressInput(bool hasError,UserControl page,DownloadProgress? progress)
	{
		_hasError= hasError;
		_page= page;
		_progress= progress;
	}

}
