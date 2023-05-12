using Avalonia.Threading;
using Avalonia.X11;
using CoreLaunching.Forge;
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
                                if (versionInfo.FileMajorPart == _majorJ)
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
                            File.WriteAllText(localJson,combined);
                            localMCJson = localJson;
                            cltlocal = localJson.Replace(".json",".jar");
                            var content2 = ForgeParser.GetInstallProfileContentFromInstaller(url, true);
                            combined2 = ForgeParser.CombineInstallerProfileJson(combined, content2, ParseType.Json);
                        }
                        var parser = new Parser();
#if WPF
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
                        TotalSize = 0;
                        List<MCFileInfo> lst;
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
                        if(Arguments is InstallArgsWithForge)
                        {
                            processm.Finished += Processm_Finished;
                        }
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

        private void Processm_Finished(object? sender, EventArgs e)
        {
            CurrectProgressIndex = 2;
            Content = "安装 Forge 中";
            var installer = new ForgeInstaller();
            installer.Output += Installer_Output;
            installer.InstallClient(_jaPth, Path.Combine(DotMCPath,"libraries"),
                cltlocal,localMCJson,inslocal,ParseType.FilePath);
            CurrectProgressIndex= 3;
            Content = "已完成！";
        }

        private void Installer_Output(object? sender, string e)
        {
            Debug.WriteLine(e);
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
        public PinKcatSingleProcess(string nativePath,string localPath)
        {
            NativeUrl= nativePath;
            LocalPath= localPath;
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override void Continue()
        {
            if (Statu == DownloadProgressState.Downloading)
            {
                ThreadPool.SetMaxThreads(512, 512);
                var info = new MCFileInfo(NativeUrl,"",TotalSize,NativeUrl,LocalPath);
                if (TotalSize > 2500000)
                {
                    var p = MutilFileDownloadProcess.Create(info,Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),$"[CL]Tempfor{Path.GetFileNameWithoutExtension(NativeUrl)}"));
                    //p.OnePartFinished += P_DownloadedUpdated;
                    p.CombineFinished += P_CombineFinished;
                    try
                    {
                        foreach (var t in p.Requsets)
                        {
                            t.Thread.Start();
                            t.OnePartFinished += P_DownloadedUpdated;
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
            throw new NotImplementedException();
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
                        TotalSize= rep.ContentLength;
                    }
                    Statu = DownloadProgressState.Downloading;
                    Continue();
                }
                catch
                {
                    Fail();
                }
            }).Start();
        }
    }
}
