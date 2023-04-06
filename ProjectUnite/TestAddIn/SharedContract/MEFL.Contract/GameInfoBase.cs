using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
#if NET4_0
#else
using System.Runtime.CompilerServices;
#endif
using System.Windows;
#if WPF
using System.Windows.Media;
#elif AVALONIA
using Avalonia.Media;
using FrameworkElement = Avalonia.Controls.Control;
#endif
using MEFL.Arguments;
using Newtonsoft.Json;

namespace MEFL.Contract;

[JsonConverter(typeof(GameInfoConverter))]
public abstract class GameInfoBase : MEFLClass,INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
#if NET4_0

#else
	/// <summary>
	/// ֪ͨ����������ݣ�MVVM��
	/// </summary>
	/// <param name="prop">������</param>
	public void PropChanged([CallerMemberName] string prop = "")
	{
		PropertyChanged?.Invoke(this,new(prop));
	}	
#endif

	public override string ToString()
	{
		return $"{Version} {VersionType}";
	}
	/// <summary>
	/// ��Ҫ���ص��ļ�������ʱ�ᴴ������������ɲ���������
	/// </summary>
    public abstract List<JsonFileInfo> FileNeedsToDownload { get; set; }
	/// <summary>
	/// ��Ҫ��ѹ���ļ���Natives����
	/// </summary>
    public abstract List<JsonFileInfo> NativeFilesNeedToDepackage { get; set; }
	/// <summary>
	/// .minecraft ·��(Ҳ���Բ���.minecraft)
	/// </summary>
	public string dotMinecraftPath => Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(GameJsonPath)));
	/// <summary>
	/// HeapDumpPath, �� Java ���м�ע��
	/// </summary>
	public abstract string HeapDumpPath { get; }
	/// <summary>
	/// ��Դ��������
	/// </summary>
	public abstract string AssetsIndexName { get; }
	/// <summary>
	/// �汾����
	/// </summary>
	public abstract string VersionType { get; }
	/// <summary>
	/// �����Ϸ�ڴ�
	/// </summary>
	public abstract int GameMaxMem { get; set; }
	/// <summary>
	/// ��С��Ϸ�ڴ�
	/// </summary>
	public abstract int GameMinMem { get; set; }
	/// <summary>
	/// ClassPaths�� һ��� Json ������Եõ���
	/// </summary>
	public abstract List<string> ClassPaths { get; }
	/// <summary>
	/// ��������Json ������
	/// </summary>
	public abstract string MainClassName { get; }
	/// <summary>
	/// ��Ϸ Json ��·��
	/// </summary>
	public abstract string GameJsonPath { get; set; }
    /// <summary>
    /// ��Ϸ�Ѻ����ƣ��׳����ø� MEFL ����Ϸ�����׳ƣ��磨�����հ桱������ʽ�桱����
    /// </summary>
    public abstract string GameTypeFriendlyName { get; set; }
	/// <summary>
	/// ������Json ����û�У����� MEFL �汾�б������������Ϣ������
	/// </summary>
	public abstract string Description { get; set; }
	/// <summary>
	/// �汾�ţ�Json ������
	/// </summary>
	public abstract string Version { get; set; }
	/// <summary>
	/// ��Ϸ�ͻ���·��
	/// </summary>
	public abstract string GameJarPath { get; }
	/// <summary>
	/// ��Ϸ���ƣ�����ָ�ļ�������
	/// </summary>
	public string Name => Path.GetFileName(GameJsonPath.Replace(".json", string.Empty));
    /// <summary>
    /// ͼ�꣬Json û��
    /// </summary>
#if WPF

	public abstract ImageSource IconSource { get; }
#elif AVALONIA
    public abstract IImage IconSource { get; }
#endif
    /// <summary>
    /// ��Ϸ��·����Json û�У����ص���Path.Combine(dotMinecraftPath,"libraries");
    /// </summary>
    public string LibrariesPath => Path.Combine(dotMinecraftPath,"libraries");
	/// <summary>
	/// ��ѹ�� Native ���Ŀ¼��λ��
	/// </summary>
	public abstract string NativeLibrariesPath { get; set; }
	/// <summary>
	/// ��Ϸ������Json �ҵ���
	/// </summary>
	public abstract string GameArgs { get; }
	/// <summary>
	/// JVM ������Json Ҳ�У�
	/// </summary>
	public abstract string JVMArgs { get; }
	/// <summary>
	/// ������Ϸ���������ʵ�֣�
	/// </summary>
	public abstract string OtherGameArgs { get; set; }
    /// <summary>
    /// ���� JVM ���������ʵ�֣�
    /// </summary>
    public abstract string OtherJVMArgs { get; set; }
	/// <summary>
	/// ��С�� Java �汾��
	/// </summary>
	public abstract int JavaMajorVersion { get; }
	/// <summary>
	/// ��Ϸ�ļ��У���ͼ��ģ����ļ��У�
	/// </summary>
	public abstract string GameFolder { get; set; }
	/// <summary>
	/// Json ������λ��
	/// </summary>
	public string RootFolder => Path.GetDirectoryName(GameJsonPath);
	/// <summary>
	/// ��Ϸ����� UI ����
	/// </summary>
	public abstract IGameSettingPage SettingsPage { get; }
	/// <summary>
	/// ˢ����Ϸ��ÿ�ο�ʼ�����ͻ�ִ��������Ĵ���
	/// </summary>
	public abstract void Refresh();
	/// <summary>
	/// ɾ����Ϸ��ͨ��������д�¼�
	/// </summary>
	/// <returns>�Ƿ�ȷ��ɾ���Ի���</returns>
	public abstract DeleteResult Delete();
	/// <summary>
	/// ���������ҳ��
	/// </summary>
	/// <param name="process">�������</param>
	/// <param name="args">���ò���</param>
	/// <returns>ҳ��</returns>
	public abstract FrameworkElement GetManageProcessPage(Process process, SettingArgs args);
	/// <summary>
	/// ǿ��ɾ��
	/// </summary>
}

public enum DeleteResult 
{ 
	OK,
	Canceled
}