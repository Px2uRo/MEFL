using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.CLAddIn.Downloaders
{
    public class NormalDownloader : MEFLDownloader
    {
        public override string Name => "NormalDownloader";

        public override string Description => "普通下载器";

        public override Version Version => new(0, 0, 0);

        public override object Icon => "C#";

        public override DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, DownloadSource[] Sources,string dotMCFolder)
        {
            return new NormalDownloadProgress(NativeUrl,LoaclPath,dotMCFolder);
        }

        public override DownloadProgress CreateProgress(List<NativeLocalPair> NativeLocalPairs, DownloadSource[] Sources,string dotMCFolder)
        {
            return new NormalDownloadProgress(NativeLocalPairs,dotMCFolder);
        }
    }
}
