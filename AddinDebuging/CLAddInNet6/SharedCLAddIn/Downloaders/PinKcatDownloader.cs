
using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.CLAddIn.Downloaders
{
    internal class PinKcatDownloader : MEFLDownloader
    {
        public override string Name => "PinKcatDownloader";

        public override string Description => "PinKcatDownloader";

        public override Version Version => new(0, 0, 0);

        public override object Icon => "P";

        public override InstallProcess CreateProgress(string NativeUrl, string LoaclPath, DownloadSource[] sources, string usingLocalFiles)
        {
            throw new NotImplementedException();
        }

        public override InstallProcess CreateProgress(NativeLocalPairsList NativeLocalPairs, DownloadSource[] sources, string usingLocalFiles)
        {
            throw new NotImplementedException();
        }

        public override InstallProcess InstallMinecraft(string jsonSource, string dotMCFolder, DownloadSource[] sources, InstallArguments args, string[] usingLocalFiles)
        {
            return PinKcatProcess.CreateInstall(jsonSource, dotMCFolder, sources, args,usingLocalFiles);
        }
    }
}
