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
	/// ��������
	/// </summary>
	/// <param name="downloader">������</param>
	/// <param name="dotMCFolder">.minecraft�ļ���</param>
	/// <param name="args">����</param>
	/// <param name="sources">����Դ</param>
	/// <param name="usingLocalFiles">���������еı����ļ���Avalonia ��δʵ�֣�</param>
	/// <param name="page">���Ϊ false �������Page</param>
	/// <returns>�Ƿ��������</returns>
	public abstract bool DirectDownload(FileInfo[] Javas,string dotMCPath,out IInstallPage page,out InstallArguments args);
}

public abstract class LauncherWebVersionContext
{
	public abstract string Name { get; }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="downloader">������</param>
    /// <param name="dotMCFolder">.minecraft�ļ���</param>
    /// <param name="args">����</param>
    /// <param name="sources">����Դ</param>
    /// <param name="usingLocalFiles">���������еı����ļ���Avalonia ��δʵ�֣�</param>
    /// <param name="page">���Ϊ false �������Page</param>
    /// <returns>�Ƿ��������</returns>
    public abstract bool DirectDownload(string url,FileInfo[] Javas, string dotMCPath, out IInstallPage page, out InstallArguments args);
}