using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MEFL.Contract
{
    public abstract class GameInfoBase
    {
        public abstract string GameJsonPath { get; set; }
        public abstract string GameTypeFriendlyName { get; set; }
        public abstract string Description { get; set; }
        public abstract string Version { get; set; }
        public abstract string Name { get; set; }
        public abstract object IconSource { get; set; }
        public abstract string NativeLibrariesPath { get; set; }
        public abstract List<Root_Libraries> GameLibraries { get; set; }
        public abstract JavaVersion JavaVerion { get; set; }
        public abstract string GameArgs { get; set; }
        public abstract string JVMArgs { get; set; }
        public abstract string OtherGameArgs { get; set; }
        public abstract string OtherJVMArgs { get; set; }
        public abstract bool IsFavorate { get; set; }
        public abstract bool LaunchByLauncher { get; }
        public abstract Process Launch(Arguments.SettingArgs args);
        public abstract int JavaVersion { get; set; }
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
