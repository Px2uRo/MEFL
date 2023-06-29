using CLAddIn.Properties;
using CoreLaunching.DownloadAPIs.Forge;
using CoreLaunching.Forge;
using CoreLaunching.JsonTemplates;
using CoreLaunching.PinKcatDownloader;
using MEFL.Arguments;
using MEFL.Callers;
using MEFL.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using File = System.IO.File;
using static CoreLaunching.FileVerifyUtil;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using ThreadState = System.Threading.ThreadState;

namespace MEFL.CLAddIn.Downloaders
{
    internal class OringnalInstallProcess : InstallProcess
    {
        private IEnumerable<InstallArguments> _arguments;
        private string _dotMCFolder;
        private string _infop;
        private string _info;
        private Parser _parser;
        public override Task Cancel()
        {
            throw new NotImplementedException();
        }

        public override Task Continue()
        {
            throw new NotImplementedException();
        }

        List<string> _usingFiles= new List<string>();
        public override bool GetUsingLocalFiles(out IEnumerable<string> paths)
        {
            paths = _usingFiles;
            return true;
        }

        public override Task Pause()
        {
            throw new NotImplementedException();
        }

        public override Task Retry()
        {
            throw new NotImplementedException();
        }

        public override Task Start()
        {
            return Task.Factory.StartNew(() =>
            {
                this.Content = "正在获取版本 JSON 信息";
                try
                {
                    foreach (var arg in _arguments)
                    {
                        if (arg is InstallArgsWithForge forge)
                        {
                            Content = "正在合并 Forge 和 原版游戏文件信息";
                            int majorJ = 8;
                            var job = JObject.Parse(_info);
                            try
                            {
                                majorJ = Convert.ToInt32(job["javaVersion"]["majorVersion"].ToString());
                            }
                            catch { }
                            var url = BMCLForgeDownloadAPI.GetDownloadUrlFromBuild(forge.Forge.Build);
                            this.CurrentProgress = 0.1;
                            var inserp = DownloadInstaller(forge);
                            var versionInfoInIns = ForgeParser.GetVersionContentFromInstaller(inserp, false);
                            this.CurrentProgress = 0.5;
                            var combined = ForgeParser.CombineVersionJson(_info, versionInfoInIns, ParseType.Json, forge.VersionName);
                            this.CurrentProgress = 0.6;
                            File.WriteAllText(_infop, combined);
                            this.CurrentProgress = 1;
                            CurrectProgressIndex++;
                            Content = "正在下载 Forge 游戏库";
                            var f = DownloadLib(arg, versionInfoInIns);
                            f.Files = f.Files.Verify();
                            foreach (var file in f.Files)
                            {
                                _usingFiles.Add(file.Local);
                            }
                            f.OneFileFinished += O_OneFileFinished;
                            f.DownloadedSizeUpdated += O_DownloadedSizeUpdated;
                            f.Start(Path.Combine(_dotMCFolder, "CoreLaunchingTemp"));
                            while (f.DownloadedSize!=f.TotalSize)
                            {
                                Thread.Sleep(100);
                            };
                            CurrectProgressIndex++;

                            Content = "正在下载 Forge 安装库";
                            var InstProfInfos = ForgeParser.GetInstallProfileContentFromInstaller(inserp, false);
                            var fi = DownloadLib(forge, InstProfInfos);
                            fi.Files = fi.Files.Verify();
                            foreach (var file in fi.Files)
                            {
                                _usingFiles.Add(file.Local);
                            }
                            fi.OneFileFinished += O_OneFileFinished;
                            fi.DownloadedSizeUpdated += O_DownloadedSizeUpdated;
                            fi.Start(Path.Combine(_dotMCFolder, "CoreLaunchingTemp"));
                            while (fi.DownloadedSize != fi.TotalSize)
                            {
                                Thread.Sleep(100);
                            };
                            CurrectProgressIndex++;

                            Content = "正在安装 Forge";
                            var inser = new ForgeInstaller();
                            string jaPth = "";
                            foreach (var item in arg.JAVAPaths)
                            {
                                var versionInfo = FileVersionInfo.GetVersionInfo(item.FullName);
                                if (versionInfo.FileMajorPart == majorJ && item.FullName.EndsWith("javaw.exe"))
                                {
                                    jaPth = item.FullName; break;
                                }
                            }
                            inser.InstallClient(jaPth, Path.Combine(_dotMCFolder, "libraries"),
                _infop.Replace(".json", ".jar"), _infop, inserp, ParseType.FilePath);
                            CurrectProgressIndex++;
                        }
                        else if (arg is InstallArguments args)
                        {
                            _info = GetContent(args.Info.Url);
                            CurrentProgress = 0.5;
                            _infop = Path.Combine(_dotMCFolder, "versions",arg.VersionName,arg.VersionName+".json");
                            Directory.CreateDirectory(Path.GetDirectoryName(_infop));
                            using (var fs = File.CreateText(_infop))
                            {
                                fs.Write(_info);
                            }
                            CurrentProgress = 1;
                            CurrectProgressIndex++;
                            this.Content = "正在下载 原版 游戏与支持库";
                            //AssetsDownloadProcess.AddItemsFromVersionInfo(_info,_parser);
                            var o = DownloadLib(arg, _info);
                            foreach (var file in o.Files)
                            {
                                _usingFiles.Add(file.Local);
                            }
                            o.OneFileFinished += O_OneFileFinished;
                            o.DownloadedSizeUpdated += O_DownloadedSizeUpdated;
                            o.Start(Path.Combine(_dotMCFolder, "CoreLaunchingTemp"));
                            while (o.DownloadedSize != o.TotalSize)
                            {
                                Thread.Sleep(100);
                            };
                        }
                    }

                    CurrectProgressIndex = TotalProgresses-1;
                    Content = "正在等待资源下载完毕";

                    Finish();
                }
                catch (Exception ex)
                {
                    Fail();
                }
            });
        }

        private void O_OneFileFinished(object? sender, MCFileInfo e)
        {
            if (_usingFiles.Contains(e.Local))
            {
                _usingFiles.Remove(e.Local);
            }
        }

        private void O_DownloadedSizeUpdated(object? sender, long e)
        {
            var m = sender as ProcessManager;
            this.CurrentProgress = (double)m.DownloadedSize / (double)m.TotalSize;
        }

        private string GetContent(string url)
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadString(url);
            }
        }

        private ProcessManager DownloadLib(InstallArguments arg, string json)
        {
            var infos = _parser.ParseLibraries(json, ParseType.Json, _dotMCFolder, arg.VersionName, true);
            var libraries = new ProcessManager(infos);      
            return libraries;
        }


        //private ProcessManager DownloadClient(InstallArguments arg, string json)
        //{
        //    var infos = new MCFileInfo[] { _parser.ParseClient(json, ParseType.Json, _dotMCFolder, arg.VersionName, true) };
        //    var libraries = new ProcessManager(infos);
        //    return libraries;
        //}
        private string DownloadInstaller(InstallArgsWithForge arg)
        {
            var url = BMCLForgeDownloadAPI.GetDownloadUrlFromBuild(arg.Forge.Build);
            using (var webClient = new WebClient())
            {
                var inslocal = Path.Combine(_dotMCFolder, "versions", arg.VersionName, "[CL]InstallTemp", Path.GetFileName(url));
                Directory.CreateDirectory(Path.GetDirectoryName(inslocal));
                webClient.DownloadFile(url,inslocal);
                return inslocal;
            }
        }
        public OringnalInstallProcess(IEnumerable<InstallArguments> args,string dotMCFolder, DownloadSource[] sources)
        {
            _dotMCFolder = dotMCFolder;
            _arguments = args;
            _parser = new();
            if (sources.Count() > 0)
            {
                if (sources.Where((x) => x.ELItem == "${assets}").ToArray() != null)
                {
                    _parser.AssetsSource = sources.Where((x) => x.ELItem == "${assets}").ToArray()[0].GetUri("");
                }
                if (sources.Where((x) => x.ELItem == "${libraries}").ToArray() != null)
                {
                    _parser.LibrarySource = sources.Where((x) => x.ELItem == "${libraries}").ToArray()[0].GetUri("");
                }
                if (sources.Where((x) => x.ELItem == "${forge_libraries}").ToArray() != null)
                {
                    _parser.ForgeLibrarySource = sources.Where((x) => x.ELItem == "${forge_libraries}").ToArray()[0].GetUri("");
                }
            }
            foreach (var item in _arguments)
            {
                TotalProgresses += item.Steps;
            }
        }
    }

    internal class AssetsDownloadProcess : SizedProcess
    {
        internal static AssetsDownloadProcess Running = new();
        internal static void Show()
        {
            if (!DownloaderCaller.GetRunningTasks().Contains(Running))
            {
                DownloaderCaller.Add(Running);
            }
        }
        internal static void AddItemsFromVersionInfo(string json,Parser parser)
        {
            var r = JsonConvert.DeserializeObject<Root>(json);

        }
        public override Task Cancel()
        {
            throw new NotImplementedException();
        }

        public override Task Continue()
        {
            throw new NotImplementedException();
        }

        public override bool GetUsingLocalFiles(out IEnumerable<string> paths)
        {
            paths = new string[0];
            return true;
        }

        public override Task Pause()
        {
            throw new NotImplementedException();
        }

        public override Task Retry()
        {
            throw new NotImplementedException();
        }

        public override Task Start()
        {
            return Task.Factory.StartNew(() => { 
                
            });
        }
    }
}
