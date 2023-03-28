
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

        public override DownloadProgress CreateProgress(NativeLocalPairsList NativeLocalPairs, DownloadSource[] sources)
        {
            throw new NotImplementedException();
        }

        public override DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, DownloadSource[] sources)
        {
            throw new NotImplementedException();
        }

        public override DownloadProgress InstallMinecraft(string jsonSource, string dotMCFolder, DownloadSource[] sources, InstallArguments args)
        {
            return PinKcatProcess.CreateInstall(jsonSource,dotMCFolder,sources,args);
        }
    }
}
