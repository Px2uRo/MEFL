using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MEFL.Contract
{
    public class MEFLErrorType : Contract.GameInfoBase
    {
        public override string GameJarPath => throw new NotImplementedException();
        public override string GameJsonPath { get ; set; }
        public override string GameTypeFriendlyName { get; set; }
        public override string Description { get; set; }
        public override string Version { get; set; }
        public override ImageSource IconSource { get;  }
        public override string NativeLibrariesPath { get; set; }
        public override string GameArgs { get; }
        public override string JVMArgs { get; }
        public override string OtherGameArgs { get; set; }
        public override string OtherJVMArgs { get; set; }
        public override string GameFolder { get; set; }

        public override FrameworkElement GetManageProcessPage(Process process, SettingArgs args)
        {
            throw new NotImplementedException();
        }
        //todo 设置页面
        public override FrameworkElement SettingsPage => throw new NotImplementedException();

        public override string HeapDumpPath => throw new NotImplementedException();

        public override int GameMaxMem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int GameMinMem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override List<string> ClassPaths => throw new NotImplementedException();

        public override string MainClassName => throw new NotImplementedException();

        public override int JavaMajorVersion => throw new NotImplementedException();

        public override string AssetsRoot => throw new NotImplementedException();

        public override string AssetsIndexName => throw new NotImplementedException();

        public override string VersionType => throw new NotImplementedException();

        public override List<LauncherWebFileInfo> FileNeedsToDownload { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override List<LauncherWebFileInfo> NativeFilesNeedToDepackage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override List<string> ItemsNeedsToExtract => throw new NotImplementedException();

        public override void Dispose()
        {

        }

        public override void Delete()
        {
            //todo Delete This
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        public MEFLErrorType(string ErrorDescription,string JsonPath)
        {
            Description=ErrorDescription;
            GameJsonPath = JsonPath;
        }
    }
}
