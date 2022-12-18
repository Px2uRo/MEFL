﻿using MEFL.Contract;
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

        public override DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, Dictionary<string, List<DownloadSource>> sources, string dotMCFolder)
        {
            return new NormalDownloadProgress(NativeUrl, LoaclPath, dotMCFolder);
        }

        public override DownloadProgress CreateProgress(List<NativeLocalPair> NativeLocalPairs, Dictionary<string, List<DownloadSource>> sources, string dotMCFolder)
        {
            return new NormalDownloadProgress(NativeLocalPairs, dotMCFolder);
        }
    }
    public class CLDownloader : MEFLDownloader
    {
        public override string Name => "CoreLaunchingDownloader";

        public override string Description => "CoreLaunching的多线程下载器";

        public override Version Version => new(0, 0, 0);

        public override object Icon => "CL";

        public override DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, Dictionary<string, List<DownloadSource>> sources, string dotMCFolder)
        {
            return new CLDownloadProgress(NativeUrl, LoaclPath, dotMCFolder);
        }

        public override DownloadProgress CreateProgress(List<NativeLocalPair> NativeLocalPairs, Dictionary<string, List<DownloadSource>> sources, string dotMCFolder)
        {
            return new CLDownloadProgress(NativeLocalPairs, dotMCFolder);
        }
    }
}
