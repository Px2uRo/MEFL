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
	/// 通知程序更新数据（MVVM）
	/// </summary>
	/// <param name="prop">属性名</param>
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
	/// 需要下载的文件，启动时会创建任务，任务完成才能启动。
	/// </summary>
    public abstract List<JsonFileInfo> FileNeedsToDownload { get; set; }
	/// <summary>
	/// 需要解压的文件（Natives）。
	/// </summary>
    public abstract List<JsonFileInfo> NativeFilesNeedToDepackage { get; set; }
	/// <summary>
	/// .minecraft 路径(也可以不叫.minecraft)
	/// </summary>
	public string dotMinecraftPath => Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(GameJsonPath)));
	/// <summary>
	/// HeapDumpPath, 求 Java 大佬加注释
	/// </summary>
	public abstract string HeapDumpPath { get; }
	/// <summary>
	/// 资源索引名称
	/// </summary>
	public abstract string AssetsIndexName { get; }
	/// <summary>
	/// 版本类型
	/// </summary>
	public abstract string VersionType { get; }
	/// <summary>
	/// 最大游戏内存
	/// </summary>
	public abstract int GameMaxMem { get; set; }
	/// <summary>
	/// 最小游戏内存
	/// </summary>
	public abstract int GameMinMem { get; set; }
	/// <summary>
	/// ClassPaths， 一般从 Json 里面可以得到。
	/// </summary>
	public abstract List<string> ClassPaths { get; }
	/// <summary>
	/// 主类名，Json 里面有
	/// </summary>
	public abstract string MainClassName { get; }
	/// <summary>
	/// 游戏 Json 的路径
	/// </summary>
	public abstract string GameJsonPath { get; set; }
    /// <summary>
    /// 游戏友好名称（俗称设置给 MEFL 的游戏类型俗称，如（“快照版”、“正式版”））
    /// </summary>
    public abstract string GameTypeFriendlyName { get; set; }
	/// <summary>
	/// 描述，Json 里面没有，是在 MEFL 版本列表里面的起辅助信息的作用
	/// </summary>
	public abstract string Description { get; set; }
	/// <summary>
	/// 版本号，Json 里面有
	/// </summary>
	public abstract string Version { get; set; }
	/// <summary>
	/// 游戏客户端路径
	/// </summary>
	public abstract string GameJarPath { get; }
	/// <summary>
	/// 游戏名称，这里指文件夹名称
	/// </summary>
	public string Name => Path.GetFileName(GameJsonPath.Replace(".json", string.Empty));
    /// <summary>
    /// 图标，Json 没有
    /// </summary>
#if WPF

	public abstract ImageSource IconSource { get; }
#elif AVALONIA
    public abstract IImage IconSource { get; }
#endif
    /// <summary>
    /// 游戏库路径，Json 没有，返回的是Path.Combine(dotMinecraftPath,"libraries");
    /// </summary>
    public string LibrariesPath => Path.Combine(dotMinecraftPath,"libraries");
	/// <summary>
	/// 解压的 Native 库的目录的位置
	/// </summary>
	public abstract string NativeLibrariesPath { get; set; }
	/// <summary>
	/// 游戏参数（Json 找到）
	/// </summary>
	public abstract string GameArgs { get; }
	/// <summary>
	/// JVM 参数（Json 也有）
	/// </summary>
	public abstract string JVMArgs { get; }
	/// <summary>
	/// 附加游戏参数（插件实现）
	/// </summary>
	public abstract string OtherGameArgs { get; set; }
    /// <summary>
    /// 附加 JVM 参数（插件实现）
    /// </summary>
    public abstract string OtherJVMArgs { get; set; }
	/// <summary>
	/// 最小的 Java 版本号
	/// </summary>
	public abstract int JavaMajorVersion { get; }
	/// <summary>
	/// 游戏文件夹（地图、模组的文件夹）
	/// </summary>
	public abstract string GameFolder { get; set; }
	/// <summary>
	/// Json 所处的位置
	/// </summary>
	public string RootFolder => Path.GetDirectoryName(GameJsonPath);
	/// <summary>
	/// 游戏管理的 UI 界面
	/// </summary>
	public abstract IGameSettingPage SettingsPage { get; }
	/// <summary>
	/// 刷新游戏。每次开始启动就会执行这里面的代码
	/// </summary>
	public abstract void Refresh();
	/// <summary>
	/// 删除游戏，通常做法是写事件
	/// </summary>
	/// <returns>是否确定删除对话框</returns>
	public abstract DeleteResult Delete();
	/// <summary>
	/// 启动后管理页面
	/// </summary>
	/// <param name="process">传入进程</param>
	/// <param name="args">设置参数</param>
	/// <returns>页面</returns>
	public abstract FrameworkElement GetManageProcessPage(Process process, SettingArgs args);
	/// <summary>
	/// 强制删除
	/// </summary>
}

public enum DeleteResult 
{ 
	OK,
	Canceled
}