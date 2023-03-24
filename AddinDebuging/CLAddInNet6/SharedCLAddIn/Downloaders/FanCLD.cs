using CoreLaunching;
using CoreLaunching.Down.Web;
using CoreLaunching.JsonTemplates;
using MEFL.Arguments;
using MEFL.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using File = System.IO.File;

namespace MEFL.CLAddIn.Downloaders
{
    internal class FanCLD : MEFLDownloader
    {
        public override string Name => "FanCLD";

        public override string Description => "2.18";

        public override Version Version => new Version(0, 0, 0);

        public override object Icon => "Fanbal.png";

        public override DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, DownloadSource[] sources)
        {
            return new CLDProgre(NativeUrl, LoaclPath, sources);
        }

        public override DownloadProgress CreateProgress(NativeLocalPairsList NativeLocalPairs, DownloadSource[] sources)
        {
            return new CLDProgre(NativeLocalPairs, sources);
        }
        public override DownloadProgress InstallMinecraft(string jsonSource, string dotMCFolder, DownloadSource[] sources, InstallArguments args)
        {
            return new CLDProgre(jsonSource, dotMCFolder, sources, args);
        }
    }

    public class CLDProgre : DownloadProgress
    {
        private string nativeUrl;
        private string localPath;
        private string dotMCFolder;
        private DownloadSource[] sources;
        private bool _install = false;
        private string _version = "";
        private string _pathWithVersion = "";

        public CLDProgre(NativeLocalPairsList items, DownloadSource[] sources):base(items)
        {
            this.dotMCFolder = dotMCFolder;
            Items=items;
            this.sources = sources;
        }
        public CLDProgre(string jsonSource, string dotMCFolder, DownloadSource[] sources, InstallArguments args):base(args)
        {
            this.nativeUrl = jsonSource;
            this.dotMCFolder = dotMCFolder;
            this.sources = sources;
            if (args == null)
            {
                _version = Path.GetFileNameWithoutExtension(jsonSource);
            }
            else
            {
                _version = args.VersionName;
            }
            _pathWithVersion = Path.Combine(dotMCFolder, $"versions\\{_version}\\");
            _install = true;
        }

        public CLDProgre(string nativeUrl, string loaclPath, DownloadSource[] sources):base()
        {
            this.nativeUrl = nativeUrl;
            this.localPath = loaclPath;
            this.sources = sources;
        }

        public override void Start()
        {
            new Thread( () => 
            {
                if (_install)
                {
                    nativeUrl = SourceReplacer.Replace(nativeUrl, sources);
                    #region Parse
                    if (nativeUrl.EndsWith("json") && _install)
                    {
                        try
                        {
                            using (var webc = new WebClient())
                            {
                                #region Json&jar
                                CurrectProgress = "解析中";
                                var webstring = webc.DownloadString(nativeUrl);
                                var versionRoot = JsonConvert.DeserializeObject<Root>(webstring);
                                var jsonPath = _pathWithVersion + $"{_version}.json";
                                Directory.CreateDirectory(Path.GetDirectoryName(jsonPath));
                                File.WriteAllText(jsonPath, webstring);
                                for (int i = 0; i < sources.Length; i++)
                                {
                                    if (sources[i].ELItem == "${versions_api}")
                                    {
                                        TaskItems.Add(new(sources[i].GetUri(versionRoot.Id), _pathWithVersion + $"{_version}.jar", versionRoot.Downloads.Client.Size));
                                        break;
                                    }
                                }
                                #endregion
                                #region Assets
                                var assetIndexLocal = Path.Combine(dotMCFolder, $"assets\\indexes\\{versionRoot.AssetIndex.Id}.json");
                                AssetsObject assetRoot = null;
                                if (!File.Exists(assetIndexLocal))
                                {
                                    var assetIndexNative = versionRoot.AssetIndex.Url;
                                    assetIndexNative = SourceReplacer.Replace(assetIndexNative, sources);
                                    var assetWebString = webc.DownloadString(assetIndexNative);
                                    assetRoot = JsonConvert.DeserializeObject<AssetsObject>(assetWebString);
                                    Directory.CreateDirectory(Path.GetDirectoryName(assetIndexLocal));
                                    File.WriteAllText(assetIndexLocal, assetWebString);
                                }
                                else
                                {
                                    assetRoot = JsonConvert.DeserializeObject<AssetsObject>(File.ReadAllText(assetIndexLocal));
                                }
                                Directory.CreateDirectory(Path.Combine(dotMCFolder, $"assets\\objects\\"));
                                foreach (var item in assetRoot.Objects)
                                {
                                    var localp = Path.Combine(dotMCFolder, $"assets\\objects\\{item.Hash[..2]}\\{item.Hash}");
                                    if (!File.Exists(localp))
                                    {
                                        var native = $"http://resources.download.minecraft.net/{item.Hash[..2]}/{item.Hash}";
                                        TaskItems.Add(new(native, localp, item.Size));
                                    }
                                }
                                #endregion
                                #region Lib
                                Path.Combine(dotMCFolder, "libraries");
                                foreach (var item in versionRoot.Libraries)
                                {
                                    var native = string.Empty;
                                    var local = string.Empty;
                                    long size = 0;
                                    if (item.Downloads.Artifact != null)
                                    {
                                        native = item.Downloads.Artifact.Url;
                                        local = Path.Combine(dotMCFolder, "libraries", item.Downloads.Artifact.Path.Replace("/", "\\"));
                                        size = item.Downloads.Artifact.Size;
                                    }
                                    if (item.Downloads.Classifiers != null)
                                    {
                                        for (int j = 0; j < item.Downloads.Classifiers.Count; j++)
                                        {
                                            var classifier = item.Downloads.Classifiers[j];
                                            var classnative = classifier.Item.Url;
                                            var classlocal = Path.Combine(dotMCFolder, "libraries", classifier.Item.Path.Replace("/", "\\"));
                                            if (!System.IO.File.Exists(classlocal))
                                            {
                                                TaskItems.Add(new(classnative, classlocal, classifier.Item.Size));
                                            }
                                        }
                                    }
                                    if (!(string.IsNullOrEmpty(native) && string.IsNullOrEmpty(local)))
                                    {
                                        if (!System.IO.File.Exists(local))
                                        {
                                            TaskItems.Add(new(native, local, size));
                                        }
                                    }
                                }
                                #endregion
                                #region Download
                                List<DownloadURI> urls = new List<DownloadURI>();
                                for (int i = 0; i < TaskItems.Count; i++)
                                {
                                    TaskItems[i].NativeUrl = SourceReplacer.Replace(TaskItems[i].NativeUrl, sources);
                                    urls.Add(new(TaskItems[i].NativeUrl, TaskItems[i].LocalPath));
                                }
                                CurrectProgress = $"正在下载{Arguments.VersionName}";
                                var queue = new DownloadQueue(urls.ToArray());
                                queue.ItemsUpdated += Queue_ItemsUpdated;
                                queue.ItemFailed += Queue_ItemFailed;
                                queue.Download();
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorInfo = ex.Message;
                            State = DownloadProgressState.Failed; return;
                        }
                    }
                    #endregion
                }
                else if(Items!=null)
                {
                    var urls = new List<DownloadURI>();
                    for(int i =0; i<Items.Count; i++)
                    {
                        var item = Items[i];
                        item.NativeUrl = SourceReplacer.Replace(item.NativeUrl,sources);
                        urls.Add(new (item.NativeUrl, item.LocalPath));
                    }
                    var queue = new DownloadQueue(urls);
                    queue.ItemsUpdated += Queue_ItemsUpdated;
                    queue.ItemFailed += Queue_ItemFailed;
                    queue.Download();
                }
            }).Start();
            base.Start();
        }

        private void Queue_ItemFailed(object? sender, string e)
        {
            var ti = TaskItems.Where(x => x.NativeUrl == (sender as DownloadFile).Source.RemoteUri).ToArray()[0];
            LogWriteLine($"下载{Path.GetFileName(ti.LocalPath)}失败：{e}");
            ti.IsOver = true;
            ti.Downloaded = ti.Length;
        }

        private void Queue_ItemsUpdated(object? sender, DownloadFile e)
        {
            var ti = TaskItems.Where(x => x.NativeUrl == e.Source.RemoteUri).ToArray()[0];
            ti.IsOver = true;
            ti.Downloaded = ti.Length;
        }
    }
}
