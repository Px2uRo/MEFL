using Avalonia.Media;
using MEFL.Arguments;
using System.Collections.Generic;
using System.IO;
using System.Windows;
#if WPF
using System.Windows.Controls;
#elif AVALONIA
using UserControl = Avalonia.Controls.Control;
#endif

namespace MEFL.Contract;

public abstract class LauncherWebVersionInfo:MEFLClass
{
	public string Id { get; set; }

	public string Url { get; set; }

	public string Type { get; set; }
	public string ReleaseTime { get; set; }
	public IImage Icon { get; set; }
	public override string ToString()
	{
		return $"{Id},{Type}";
	}
	/// <summary>
	/// 调用下载
	/// </summary>
	/// <param name="downloader">下载器</param>
	/// <param name="dotMCFolder">.minecraft文件夹</param>
	/// <param name="args">参数</param>
	/// <param name="sources">下载源</param>
	/// <param name="usingLocalFiles">正在下载中的本地文件（Avalonia 暂未实现）</param>
	/// <param name="page">如果为 false 返回这个Page</param>
	/// <returns>是否可以下载</returns>
	public abstract bool DirectDownload(FileInfo[] Javas,string dotMCPath,out IInstallPage page,out InstallArguments args);
}

public abstract class LauncherWebVersionContext
{
	public abstract string Name { get; }
    /// <summary>
    /// 调用下载
    /// </summary>
    /// <param name="downloader">下载器</param>
    /// <param name="dotMCFolder">.minecraft文件夹</param>
    /// <param name="args">参数</param>
    /// <param name="sources">下载源</param>
    /// <param name="usingLocalFiles">正在下载中的本地文件（Avalonia 暂未实现）</param>
    /// <param name="page">如果为 false 返回这个Page</param>
    /// <returns>是否可以下载</returns>
    public abstract bool DirectDownload(string url,FileInfo[] Javas, string dotMCPath, out IInstallPage page, out InstallArguments args);
}