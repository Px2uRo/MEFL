using Avalonia.Threading;
using Avalonia.X11;
using CoreLaunching.Forge;
using CoreLaunching.JsonTemplates;
using CoreLaunching.MicrosoftAuth;
using CoreLaunching.PinKcatDownloader;
using MEFL.Arguments;
using MEFL.Contract;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Windows;
using CoreLaunching.DownloadAPIs.Forge;
using File = System.IO.File;
using CoreLaunching;

namespace MEFL.CLAddIn.Downloaders
{
    internal class PinKcatProcess:InstallProcess
    {
        public bool Install { get; private set; }
        public string JsonSource { get; private set; }
        public string DotMCPath { get; private set; }
        public DownloadSource[] Sources { get; private set; }
        public MCFileInfo[] ItemsArray { get; private set; }
        public string[] OtherUsingFiles { get; private set; }
        public long TotalSize = 0;
        private string _jaPth="";
        private int _majorJ;

        #region Forge
        private string localMCJson;
        private string inslocal;
        private string cltlocal;
        private string combined2;
        #endregion
        internal static InstallProcess CreateInstall(string jsonSource, string dotMCFolder, DownloadSource[] sources, InstallArguments args,string[] usingLocalFiles)
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

        List<MCFileInfo> lst;
        public override void Start()
        {
            ThreadPool.SetMaxThreads(512, 512);
            new Thread(() => {
                if (Install)
                {
                    TotalProgresses = 2;
                    CurrectProgressIndex = 0;
                    Content = "解析版本中（包括 Json 合并）";
                    try
                    {
                        var localJson = Path.Combine(DotMCPath, "versions", Arguments.VersionName,$"{Arguments.VersionName}.json");
                        Directory.CreateDirectory(Path.GetDirectoryName(localJson));
                        var webReq = HttpWebRequest.CreateHttp(JsonSource);
                        using (var respon = webReq.GetResponse())
                        {
                            TotalSize = respon.ContentLength;
                            using (var responStream = respon.GetResponseStream())
                            {
                                using(var fs = File.Open(localJson,FileMode.OpenOrCreate))
                                {
                                    byte[] buffer = new byte[1024];
                                    int bytesRead;
                                    while ((bytesRead = responStream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        fs.Write(buffer, 0, bytesRead);
                                        CurrentProgress = (double)((double)fs.Length / (double)TotalSize);
                                    }
                                    fs.Position = 0;
                                    var stri = new StreamReader(fs);
                                    var job = JObject.Parse(stri.ReadToEnd());
                                    try
                                    {
                                        _majorJ = Convert.ToInt32(job["javaVersion"]["majorVersion"].ToString());
                                    }
                                    catch
                                    {
                                        _majorJ = 8;
                                    }
                                }
                            }
                        }
                        if (Arguments is InstallArgsWithForge arg)
                        {
                            TotalProgresses = 3;
                            foreach (var item in arg.JAVAPaths)
                            {
                                var versionInfo = FileVersionInfo.GetVersionInfo(item.FullName);
                                if (versionInfo.FileMajorPart == _majorJ&&item.FullName.EndsWith("javaw.exe"))
                                {
                                    _jaPth = item.FullName; break;
                                }
                            }
                            if (string.IsNullOrEmpty(_jaPth))
                            {
                                _jaPth = arg.JAVAPaths[0].FullName;
                            }
                            var url = BMCLForgeHelper.GetDownloadUrlFromBuild(arg.Forge.Build);
                            inslocal = Path.Combine( DotMCPath,"versions",arg.VersionName,"[CL]InstallTemp",Path.GetFileName(url));
                            Directory.CreateDirectory(Path.GetDirectoryName(inslocal));
                            using (var clt = new WebClient())
                            {
                                clt.DownloadFile(url, inslocal);
                            }
                            var content = ForgeParser.GetVersionContentFromInstaller(inslocal,false);
                            var combined = ForgeParser.CombineVersionJson(File.ReadAllText(localJson),content,ParseType.Json,arg.VersionName);
                            System.IO.File.WriteAllText(localJson,combined);
                            localMCJson = localJson;
                            cltlocal = localJson.Replace(".json",".jar");
                            var content2 = ForgeParser.GetInstallProfileContentFromInstaller(url, true);
                            combined2 = ForgeParser.CombineInstallerProfileJson(combined, content2, ParseType.Json);
                        }
                        var parser = new Parser();
#if true
                        if(Sources.Count()>0)
                        {
                            if (Sources.Where((x) => x.ELItem == "${assets}").ToArray() != null)
                            {
                                parser.AssetsSource = Sources.Where((x) => x.ELItem == "${assets}").ToArray()[0].GetUri("");
                            }
                            if (Sources.Where((x) => x.ELItem == "${libraries}").ToArray() != null)
                            {
                                parser.LibrarySource = Sources.Where((x) => x.ELItem == "${libraries}").ToArray()[0].GetUri("");
                            }
                        }
#endif  
                        CurrectProgressIndex = 1;
                        Content = "下载文件中（包括资源和运行库）";
                        CurrentProgress = 0;
                        TotalSize = 0;
                        if (string.IsNullOrEmpty(combined2))
                        {
                            lst = parser.ParseFromJson(localJson,
                                ParseType.FilePath,
        DotMCPath, Arguments.VersionName, true
        ).ToList();
                        }
                        else
                        {
                            lst = parser.ParseFromJson(combined2,
    ParseType.Json,
DotMCPath, Arguments.VersionName, true
).ToList();
                        }
                        foreach (var item in lst)
                        {
                            this.TotalSize += item.Size;
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
                        var processm = new ProcessManager(lst);
                        var temp = Path.Combine(DotMCPath, "CoreLaunchingTemp");

                        processm.Finished += Processm_Finished1;
                        processm.DownloadedSizeUpdated += Processm_DownloadedSizeUpdated;
                        processm.Start(temp);
                    }
                    catch (Exception ex)
                    {
                        ErrorInfo=ex.Message;
                    }
                }
            }).Start();
        }

        DateTime part2LastUpd;
        private void Processm_Finished1(object? sender, EventArgs e)
        {
            var m = sender as ProcessManager;
            firstM = m.DownloadedSize;
            if (Arguments is InstallArgsWithForge)
            {
                var NoExistList = ItemsArray.Where(x => !File.Exists(x.Local)).ToArray();
                var processm = new ProcessManager(NoExistList);
                processm.Finished += Processm_Finished;
                processm.DownloadedSizeUpdated += Nm_DownloadedSizeUpdated;
                processm.Start(Path.Combine(DotMCPath, "CoreLaunchingTemp"), true);
            }
            else
            {
                var nm = new ProcessManager(m.Remains);
                nm.Finished += Nm_Finished;
                nm.DownloadedSizeUpdated += Nm_DownloadedSizeUpdated;
                nm.Start(Path.Combine(DotMCPath, "CoreLaunchingTemp"), true);
                part2LastUpd = DateTime.Now;
                while (DateTime.Now - part2LastUpd < TimeSpan.FromSeconds(20))
                {
                    Thread.Sleep(100);
                }
                Finish();
            }
        }
        long firstM = 0;
        private void Nm_DownloadedSizeUpdated(object? sender, long e)
        {
            part2LastUpd = DateTime.Now;
            if (CurrectProgressIndex == 1)
            {
                CurrentProgress = (double)(firstM + e )/ (double)TotalSize;
            }
        }

        private void Nm_Finished(object? sender, EventArgs e)
        {
            Finish();
        }

        private void Processm_Finished(object? sender, EventArgs e)
        {
            CurrectProgressIndex = 2;
            CurrentProgress = 0;
            Content = "安装 Forge 中";

            

            var installer = new ForgeInstaller();
            installer.Output += Installer_Output;
            installer.InstallClient(_jaPth, Path.Combine(DotMCPath,"libraries"),
                cltlocal,localMCJson,inslocal,ParseType.FilePath);
            CurrentProgress = 1;
            CurrectProgressIndex = 3;
            Content = "已完成！";
            Finish();
        }

        private void Installer_Output(object? sender, string e)
        {
            if (e == null)
            {
                return;
            }
            if (e.Contains("MCP_DATA"))
            {
                CurrentProgress = 0.05;
            }
            else if (e.Contains("DOWNLOAD_MOJMAPS"))
            {
                CurrentProgress = 0.1;
            }
            else if (e.Contains("MERGE_MAPPING"))
            {
                CurrentProgress = 0.15;
            }
            Debug.WriteLine(e);
            Content = (e);
        }

        private void Processm_DownloadedSizeUpdated(object? sender, long e)
        {
            if(CurrectProgressIndex== 1)
            {
                CurrentProgress = (double)((double)e / (double)TotalSize);
            }
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

        public override void Retry()
        {
            throw new NotImplementedException();
        }

        public override void Pause()
        {
            throw new NotImplementedException();
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override void Continue()
        {
            throw new NotImplementedException();
        }
    }

    internal class PinKcatSingleProcess : SingleProcess
    {
        Queue<RequestWithRange> total = new();
        List<RequestWithRange> running = new();
        Queue<RequestWithRange> failed = new();
        public PinKcatSingleProcess(string nativePath, string localPath)
        {
            NativeUrl = nativePath;
            LocalPath = localPath;
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override void Continue()
        {

        }

        private void T_WholeFinished(object? sender, long e)
        {
            running.Remove(sender as RequestWithRange);
        }

        private void T_Failed(object? sender, EventArgs e)
        {
            failed.Enqueue(sender as RequestWithRange);
            running.Remove(sender as RequestWithRange);
        }

        private void P_CombineFinished(object? sender, MCFileInfo e)
        {
            Finish();
        }

        private void P_Finished(object? sender, Thread e)
        {
            Finish();
        }

        private void P_DownloadedUpdated(object? sender, long e)
        {
            DownloadedSize += e;
        }

        public override bool GetUsingLocalFiles(out string[] paths)
        {
            paths = new string[0];
            return true;
        }

        public override void Pause()
        {
            throw new NotImplementedException();
        }

        public override void Retry()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            new Thread(() =>
            {
                try
                {
                    var webReq = HttpWebRequest.CreateHttp(NativeUrl);
                    using (var rep = webReq.GetResponse())
                    {
                        TotalSize = rep.ContentLength;
                    }
                    Statu = DownloadProgressState.Downloading;
                    if (Statu == DownloadProgressState.Downloading)
                    {
                        ThreadPool.SetMaxThreads(512, 512);
                        var info = new MCFileInfo(NativeUrl, "", TotalSize, NativeUrl, LocalPath);
                        if (TotalSize > 2500000)
                        {
                            var p = MutilFileDownloadProcess.Create(info,
                                Path.Combine(
                                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    $"[CL]Tempfor{Path.GetFileNameWithoutExtension(NativeUrl)}"));
                            //p.OnePartFinished += P_DownloadedUpdated;
                            p.CombineFinished += P_CombineFinished;
                            try
                            {
                                foreach (var t in p.Requsets)
                                {
                                    t.OnePartFinished += P_DownloadedUpdated;
                                    t.Failed += T_Failed;
                                    total.Enqueue(t);
                                }
                                while (total.Count > 0)
                                {
                                    if (running.Count < 64)
                                    {
                                        var t = total.Dequeue();
                                        if (t != null)
                                        {
                                            t.WholeFinished += T_WholeFinished;
                                            t.DownThread.Start();
                                            running.Add(t);
                                        }
                                    }
                                }
                                p.CombineThread.Start();
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            var p = FileDownloadProgressWithUpdate.CreateSingle(info);
                            p.DownloadedUpdated += P_DownloadedUpdated;
                            p.Finished += P_Finished;
                            p.Start();
                        }
                    }
                }
                catch
                {
                    Fail();
                }
            }).Start();
        }
    }
    internal class PinKcatPairProcess : SingleProcess
    {
        Queue<RequestWithRange> total = new();
        List<RequestWithRange> running = new();
        Queue<RequestWithRange> failed = new();
        private List<JsonFileInfo> nativeLocalPairs;
        private DownloadSource[] sources;
        private string[] usingLocalFiles;

        public PinKcatPairProcess(List<JsonFileInfo> nativeLocalPairs, DownloadSource[] sources, string[] usingLocalFiles)
        {
            this.nativeLocalPairs = nativeLocalPairs;
            this.sources = sources;
            this.usingLocalFiles = usingLocalFiles;
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override void Continue()
        {

        }

        private void T_WholeFinished(object? sender, long e)
        {
            running.Remove(sender as RequestWithRange);
        }

        private void T_Failed(object? sender, EventArgs e)
        {
            failed.Enqueue(sender as RequestWithRange);
            running.Remove(sender as RequestWithRange);
        }

        private void P_CombineFinished(object? sender, MCFileInfo e)
        {
            Finish();
        }

        private void P_Finished(object? sender, Thread e)
        {
            Finish();
        }

        private void P_DownloadedUpdated(object? sender, long e)
        {
            DownloadedSize += e;
        }

        public override bool GetUsingLocalFiles(out string[] paths)
        {
            paths = new string[0];
            return true;
        }

        public override void Pause()
        {
            throw new NotImplementedException();
        }

        public override void Retry()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            new Thread(() =>
            {
                try
                {
                    var infos = new List<MCFileInfo>();
                    foreach (var item in nativeLocalPairs)
                    {
                        infos.Add(new(Path.GetFileName(item.Url),item.sha1,item.size,item.Url,item.localpath));
                    }
                    var proms = new ProcessManager(infos);
                    var temp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"CLtemp");
                    proms.DownloadedSizeUpdated += (sender, e) =>
                    {
                        DownloadedSize = e;
                    };
                    proms.Finished += Proms_Finished;
                    new Thread(()=> { proms.Start(temp, true);}).Start();
                }
                catch
                {
                    Fail();
                }
            }).Start();
        }

        private void Proms_Finished(object? sender, EventArgs e)
        {
            Finish();
        }
    }
}
