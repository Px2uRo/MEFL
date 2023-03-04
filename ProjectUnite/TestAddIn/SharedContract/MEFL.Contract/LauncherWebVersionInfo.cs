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
/// ��������װ���
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
	/// ��װ���̽��
	/// </summary>
	/// <param name="hasError">�Ƿ��д��������û�еĻ���progress ��Ҫ��Ϊ null���еĻ�����Ϊ null</param>
	/// <param name="page">ҳ�棬MEFL ���Զ����������Ϊ���ҳ��� DataContext ���� NowDownload() �����˳�ҳ��</param>
	/// <param name="progress">����</param>
	public InstallProgressInput(bool hasError,UserControl page,DownloadProgress? progress)
	{
		_hasError= hasError;
		_page= page;
		_progress= progress;
	}

}
