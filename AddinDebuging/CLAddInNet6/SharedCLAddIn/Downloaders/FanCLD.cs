using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.CLAddIn.Downloaders
{
    internal class FanCLD : MEFLDownloader
    {
        public override string Name => "FanCLD";

        public override string Description => "2.18";

        public override Version Version => new Version(0, 0, 0);

        public override object Icon => "Fanbal.png";

        public override DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, DownloadSource[] sources, string dotMCFolder)
        {
            return new CLDPro();
        }

        public override DownloadProgress CreateProgress(List<NativeLocalPair> NativeLocalPairs, DownloadSource[] sources, string dotMCFolder)
        {
            return new CLDPro();
        }
    }

    internal class CLDPro : DownloadProgress
    {

    }
}
