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
using System.Windows.Media;
using MEFL.Arguments;

namespace MEFL.Contract;

public abstract class GameInfoBase : MEFLClass,INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
#if NET4_0

#else
	private void PropChanged([CallerMemberName] string prop = "")
	{
		PropertyChanged?.Invoke(this,new(prop));
	}	
#endif

	public override string ToString()
	{
		return $"{Version} {VersionType}";
	}
	public string AssemblyGuid => Assembly.GetAssembly(this.GetType()).ManifestModule.ModuleVersionId.ToString();

    public abstract List<string> ItemsNeedsToExtract { get; }

	public abstract List<JsonFileInfo> FileNeedsToDownload { get; set; }

	public abstract List<JsonFileInfo> NativeFilesNeedToDepackage { get; set; }

	public string dotMinecraftPath => Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(GameJsonPath)));

	public abstract string HeapDumpPath { get; }

	public abstract string AssetsIndexName { get; }

	public abstract string VersionType { get; }

	public abstract int GameMaxMem { get; set; }

	public abstract int GameMinMem { get; set; }

	public abstract List<string> ClassPaths { get; }

	public abstract string MainClassName { get; }

	public abstract string GameJsonPath { get; set; }

	public abstract string GameTypeFriendlyName { get; set; }

	public abstract string Description { get; set; }

	public abstract string Version { get; set; }

	public abstract string GameJarPath { get; }

	public string Name => Path.GetFileName(GameJsonPath.Replace(".json", string.Empty));

	public abstract ImageSource IconSource { get; }

	public string LibrariesPath => Path.Combine(dotMinecraftPath,"libraries");
	public abstract string NativeLibrariesPath { get; set; }

	public abstract string GameArgs { get; }

	public abstract string JVMArgs { get; }

	public abstract string OtherGameArgs { get; set; }

	public abstract string OtherJVMArgs { get; set; }

	public abstract int JavaMajorVersion { get; }

	public abstract string GameFolder { get; set; }

	public string RootFolder => Path.GetDirectoryName(GameJsonPath);

	public abstract IGameSettingPage SettingsPage { get; }

	public abstract void Refresh();

	public abstract DeleteResult Delete();

	public abstract FrameworkElement GetManageProcessPage(Process process, SettingArgs args);
}

public enum DeleteResult 
{ 
	OK,
	Canceled
}