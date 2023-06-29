
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

        public override SizedProcess CreateProgress(string NativeUrl, string LoaclPath, DownloadSource[] sources, string[] usingLocalFiles)
        {
            return new PinKcatSingleProcess(NativeUrl,LoaclPath);
        }

        public override SizedProcess CreateProgressFromPair(List<JsonFileInfo> NativeLocalPairs, DownloadSource[] sources, string[] usingLocalFiles)
        {
            return new PinKcatPairProcess(NativeLocalPairs,sources,usingLocalFiles);
        }

        public override InstallProcess InstallMinecraft(string jsonSource, string dotMCFolder, DownloadSource[] sources,IEnumerable<InstallArguments> args, string[] usingLocalFiles)
        {
            return new OringnalInstallProcess(args,dotMCFolder,sources);
            return PinKcatProcess.CreateInstall(jsonSource, dotMCFolder, sources, args.First(),usingLocalFiles);
        }
    }
}
