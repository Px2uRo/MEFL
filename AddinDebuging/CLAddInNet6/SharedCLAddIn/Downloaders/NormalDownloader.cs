using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.CLAddIn.Downloaders
{
    public class NormalDownloader : MEFLDownloader
    {
        public override string Name => "WebClientC#";

        public override string Description => "WebClientC#";

        public override Version Version => new(0, 0, 0);

        public override object Icon => "C#";

        public override DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, IEnumerable<DownloadSource> sources, string dotMCFolder)
        {
            return new NormalDownloadProgress(NativeUrl, LoaclPath, dotMCFolder, new List<DownloadSource>(sources).ToArray());
        }

        public override DownloadProgress CreateProgress(NativeLocalPair nativeLocalPair, IEnumerable<DownloadSource> sources, string doyMCFolder)
        {
            throw new NotImplementedException();
        }

        public override DownloadProgress CreateProgress(IEnumerable<NativeLocalPair> NativeLocalPairs, IEnumerable<DownloadSource> sources, string dotMCFolder)
        {
            return new NormalDownloadProgress(new List<NativeLocalPair>(NativeLocalPairs), dotMCFolder, new List<DownloadSource>( sources).ToArray());
        }
    }
  

}
