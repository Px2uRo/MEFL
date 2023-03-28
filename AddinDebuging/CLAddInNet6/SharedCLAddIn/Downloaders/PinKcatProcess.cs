using CoreLaunching.PinKcatDownloader;
using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace MEFL.CLAddIn.Downloaders
{
    internal class PinKcatProcess:DownloadProgress
    {
        public bool Install { get; private set; }
        public string JsonSource { get; private set; }
        public string DotMCPath { get; private set; }
        public DownloadSource[] Sources { get; private set; }
        

        internal static DownloadProgress CreateInstall(string jsonSource, string dotMCFolder, DownloadSource[] sources, InstallArguments args)
        {
            var res = new PinKcatProcess();
            res.Install = true;
            res.JsonSource = jsonSource;
            res.DotMCPath = dotMCFolder;
            res.Sources = sources;
            res.Arguments= args;
            return res;
        }

        public override void Start()
        {
            ThreadPool.SetMaxThreads(512, 512);
            new Thread(() => {
                if (Install)
                {
                    try
                    {
                        var localJson = Path.Combine(DotMCPath, "versions", Arguments.VersionName,$"{Arguments.VersionName}.json");
                        Directory.CreateDirectory(Path.GetDirectoryName(localJson));
                        using (var clt = new WebClient())
                        {
                            clt.DownloadFile(JsonSource,localJson);
                        }
                        var arr = Parser.ParseFromJson(localJson,
                            Parser.ParseType.FilePath,
    DotMCPath, Arguments.VersionName, true
    );
                        foreach (var item in arr)
                        {
                            TotalCount++;
                            TotalSize += item.Size;
                        }
                        var superSmall = arr.Where((x) => x.Size < 250000).ToArray();
                        var small = arr.Where((x) => x.Size <= 2500000&&x.Size>= 250000).ToArray();
                        var large = arr.Where((x) => x.Size > 2500000).ToArray();
                        var promss = new SuperSmallProcessManager(superSmall);
                        promss.OneFinished += Proms_OneFinished;
                        promss.QueueEmpty += Promss_QueueEmpty;
                        new Thread(() => promss.DownloadSingle()).Start();
                        var proms = new SingleThreadProcessManager(small);
                        proms.OneFinished += Proms_OneFinished;
                        proms.QueueEmpty += Promss_QueueEmpty;
                        new Thread(() => proms.DownloadSingle()).Start();
                        var prom = new MutilFileDownloaManager(large);
                        var temp = Path.Combine(DotMCPath,"CoreLaunchingTemp");
                        Directory.CreateDirectory(temp);
                        prom.OneFinished += Proms_OneFinished;
                        prom.QueueEmpty += Promss_QueueEmpty;
                        new Thread(() => prom.Download(temp)).Start();
                    }
                    catch (Exception ex)
                    {
                        ErrorInfo=ex.Message;
                        this.State = DownloadProgressState.Failed;
                    }
                }
            }).Start();
        }

        private void Promss_QueueEmpty(object? sender, EventArgs e)
        {
            LogWriteLine("其中一个下载 Queue 完成（共有三个 Queue）稍等一会（直到三个都完成，且进度条没有动的时候）应该就可以安全退出了");
        }

        private void Proms_OneFinished(object? sender, MCFileInfo e)
        {
            DownloadedItems++;
            DownloadedSize += e.Size;
        }
    }
}
