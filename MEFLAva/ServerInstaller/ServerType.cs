using Avalonia.Media;
using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInstaller
{
    internal class ServerType : GameInfoBase
    {
        public override List<JsonFileInfo> FileNeedsToDownload { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override List<JsonFileInfo> NativeFilesNeedToDepackage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override string HeapDumpPath => throw new NotImplementedException();

        public override string AssetsIndexName => throw new NotImplementedException();

        public override string VersionType => throw new NotImplementedException();

        public override int GameMaxMem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int GameMinMem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override List<string> ClassPaths => throw new NotImplementedException();

        public override string MainClassName => throw new NotImplementedException();

        public override string GameJsonPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string GameTypeFriendlyName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Version { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override string GameJarPath => throw new NotImplementedException();

        public override IImage IconSource => throw new NotImplementedException();

        public override string NativeLibrariesPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override string GameArgs => throw new NotImplementedException();

        public override string JVMArgs => throw new NotImplementedException();

        public override string OtherGameArgs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string OtherJVMArgs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int JavaMajorVersion => throw new NotImplementedException();

        public override string GameFolder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IGameSettingPage SettingsPage => throw new NotImplementedException();

        public override DeleteResult Delete()
        {
            throw new NotImplementedException();
        }

        public override IProcessManagePage GetManageProcessPage(Process process, SettingArgs args)
        {
            throw new NotImplementedException();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
