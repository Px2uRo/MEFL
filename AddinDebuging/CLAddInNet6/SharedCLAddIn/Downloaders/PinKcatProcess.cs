using Avalonia.Threading;
using CoreLaunching.Forge;
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
using System.Windows;

namespace MEFL.CLAddIn.Downloaders
{
    internal class PinKcatProcess:DownloadProgress
    {
        public bool Install { get; private set; }
        public string JsonSource { get; private set; }
        public string DotMCPath { get; private set; }
        public DownloadSource[] Sources { get; private set; }
        public MCFileInfo[] ItemsArray { get; private set; }
        public string[] OtherUsingFiles { get; private set; }

        internal static DownloadProgress CreateInstall(string jsonSource, string dotMCFolder, DownloadSource[] sources, InstallArguments args,string[] usingLocalFiles)
        {
            var res = new PinKcatProcess();
            res.Install = true;
            res.JsonSource = jsonSource;
            res.DotMCPath = dotMCFolder;
            res.Sources = sources;
            res.Arguments= args;
            res.OtherUsingFiles = usingLocalFiles;
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
                        if (Arguments is InstallArgsWithForge)
                        {
                            var arg = Arguments as InstallArgsWithForge;
                            var url = BMCLForgeHelper.GetDownloadUrlFromBuild(arg.Forge.Build);
                            var content = ForgeParser.GetVersionContentFromInstaller(url,true);
                            var combined = ForgeParser.CombineJson(File.ReadAllText(localJson),content,ParseType.Json);
                            File.WriteAllText(localJson,combined);
                        }
                        var parser = new Parser();
                        parser.AssetsSource = Sources.Where((x)=>x.ELItem== "${assets}").ToArray()[0].GetUri("");
                        parser.LibrarySource = Sources.Where((x) => x.ELItem == "${libraries}").ToArray()[0].GetUri("");
                        var lst = parser.ParseFromJson(localJson,
                            ParseType.FilePath,
    DotMCPath, Arguments.VersionName, true
    ).ToList();
                        foreach (var item in lst)
                        {
                            TotalCount++;
                            TotalSize += item.Size;
                        }
                        ItemsArray = lst.ToArray();
                        for (int i = 0; i < OtherUsingFiles.Length; i++)
                        {
                            var combinedLq = lst.Where((x) => x.Local == OtherUsingFiles[i]).ToArray();
                            if (combinedLq.Length > 0)
                            {
                                lst.Remove(combinedLq[0]);
                            }
                        }
                        var superSmall = lst.Where((x) => x.Size < 250000).ToArray();
                        var small = lst.Where((x) => x.Size <= 2500000&&x.Size>= 250000).ToArray();
                        var large = lst.Where((x) => x.Size > 2500000).ToArray();
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

        int finishedmgr = 0;
        private void Promss_QueueEmpty(object? sender, EventArgs e)
        {
            finishedmgr++;
            if(finishedmgr == 3) 
            {
#if WPF
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    State = DownloadProgressState.Finished;
                }));
#elif AVALONIA
                Dispatcher.UIThread.InvokeAsync((() => 
                {
                    State = DownloadProgressState.Finished;
                }));
#endif
            }
        }

        private void Proms_OneFinished(object? sender, MCFileInfo e)
        {
            DownloadedItems++;
            DownloadedSize += e.Size;
        }

        public override bool GetUsingLocalFiles(out string[] paths)
        {
            var lst = new List<string>();
            if (ItemsArray == null)
            {
                    paths = null;
                    return false;
            }
            else
            {
                for (int i = 0; i < ItemsArray.Length; i++)
                {
                    lst.Add(ItemsArray[i].Local);
                }
                paths = lst.ToArray();
                return true;
            }
        }
    }
}
