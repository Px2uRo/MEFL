using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
#if WINDOWS
using System.Windows;
using System.Windows.Media;
#elif AVALONIA
using Avalonia.Controls;
using Avalonia.Media;
#endif

namespace MEFL.Contract
{
    public abstract class GameInfoBase:IDisposable
    {
        public string dotMinecraftPath 
        { 
            get 
            {
                return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(GameJsonPath)));
            } 
        }
        public abstract string HeapDumpPath { get; }
        public abstract string AssetsRoot { get; }
        public abstract string AssetsIndexName { get; }
        public abstract string VersionType { get; }
        public abstract int GameMaxMem { get; set; }
        public abstract int GameMinMem { get; set; }
        public abstract List<String> ClassPaths { get; }
        public abstract string MainClassName { get; }
        public abstract string GameJsonPath { get; set; }
        public abstract string GameTypeFriendlyName { get; set; }
        public abstract string Description { get; set; }
        public abstract string Version { get; set; }
        public abstract string GameJarPath { get; }
        public string Name { get 
            { 
                return Path.GetFileName(GameJsonPath.Replace(".json",string.Empty));
            } 
        }
        #region Icon
        public abstract
#if WINDOWS
            ImageSource 
#elif AVALONIA
    IImage
#endif
                IconSource
        { get; }
        #endregion

        public abstract string NativeLibrariesPath { get; set; }
        public abstract List<Root_Libraries> GameLibraries { get; set; }
        public abstract string GameArgs { get; }
        public abstract string JVMArgs { get; }
        public abstract string OtherGameArgs { get; set; }
        public abstract string OtherJVMArgs { get; set; }
        public abstract bool LaunchByLauncher { get; }
        public abstract Process Launch(Arguments.SettingArgs args);

        public abstract void Dispose();
        public abstract void Delete();

        public abstract int JavaMajorVersion { get; }
        public abstract string GameFolder { get; set; }
        public string RootFolder { get => Path.GetDirectoryName(GameJsonPath); }
        public abstract FrameworkElement GetManageProcessPage(Process process,Arguments.SettingArgs args);
        public abstract FrameworkElement SettingsPage { get; }
    }

    public class JavaVersion
    {
        public string Component { get; set; }
        public int MajorVersion { get; set; }
    }
    public class Root_Libraries
    {
        public Root_Downloads Downloads { get; set; }

        public string Name { get; set; }
        public string native { get; set; }
    }

    public class Root_Downloads
    {
        public Root_AFileBase Artifact { get; set; }
        public Root_AFileBase Classifiers { get; set; }
        public string Name { get; set; }
    }
    public class Root_AFileBase
    {
        public string path { get; set; }
        public string sha1 { get; set; }
        public int size { get; set; }
        public string url { get; set; }
    }
}
