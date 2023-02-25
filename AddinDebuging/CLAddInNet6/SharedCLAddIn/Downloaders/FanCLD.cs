using CLAddInNet6.Properties;
using CoreLaunching;
using CoreLaunching.Down.Web;
using CoreLaunching.JsonTemplates;
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

        public override DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, DownloadSource[] sources, string dotMCFolder)
        {
            return new CLDProgre(NativeUrl, LoaclPath, dotMCFolder, sources);
        }

        public override DownloadProgress CreateProgress(NativeLocalPairsManager NativeLocalPairs, DownloadSource[] sources, string dotMCFolder)
        {
            return new CLDProgre(NativeLocalPairs, dotMCFolder, sources);
        }
        public override DownloadProgress InstallMinecraft(string jsonSource, string dotMCFolder, DownloadSource[] sources, object? parama)
        {
            return new CLDProgre(jsonSource, dotMCFolder, sources, parama);
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

        public CLDProgre(NativeLocalPairsManager manager, string dotMCFolder, DownloadSource[] sources):base(manager)
        {
            this.dotMCFolder = dotMCFolder;
            this.sources = sources;
        }
        public CLDProgre(string jsonSource, string dotMCFolder, DownloadSource[] sources, object? parama):base()
        {
            this.nativeUrl = jsonSource;
            this.dotMCFolder = dotMCFolder;
            this.sources = sources;
            if (parama == null)
            {
                this._version = Path.GetFileNameWithoutExtension(jsonSource);
                _pathWithVersion = Path.Combine(dotMCFolder, $"versions\\{_version}\\");
                //this.NativeLocalPairs.Add(new(jsonSource, _pathWithVersion +$"{_version}.json"));
                _install = true;
            }
            else
            {
                _install = true;
            }
        }

        public CLDProgre(string nativeUrl, string loaclPath, string dotMCFolder, DownloadSource[] sources):base()
        {
            this.nativeUrl = nativeUrl;
            this.localPath = loaclPath;
            this.dotMCFolder = dotMCFolder;
            this.sources = sources;
        }

        public override void Start()
        {
            new Thread(() => 
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
                            var webstring = webc.DownloadString(nativeUrl);
                            var versionRoot = JsonConvert.DeserializeObject<Root>(webstring);
                            var jsonPath = _pathWithVersion + $"{_version}.json";
                            File.WriteAllText(jsonPath, webstring);
                            TaskItems.Add(new(versionRoot.Downloads.Client.Url, _pathWithVersion + $"{_version}.jar", versionRoot.Downloads.Client.Size));
                            #endregion
                            #region Assets
                            var assetIndexNative = versionRoot.AssetIndex.Url;
                            assetIndexNative = SourceReplacer.Replace(assetIndexNative, sources);
                            var assetIndexLocal = Path.Combine(dotMCFolder, $"assets\\indexes\\{versionRoot.AssetIndex.Id}.json");
                            var assetWebString = webc.DownloadString(assetIndexNative);
                            var assetRoot = JsonConvert.DeserializeObject<AssetsObject>(assetWebString);
                            File.WriteAllText(assetIndexLocal, assetWebString);
                            foreach (var item in assetRoot.Objects)
                            {
                                var native = $"http://resources.download.minecraft.net/{item.Hash[..2]}/{item.Hash}";
                                var localp = Path.Combine(dotMCFolder, "assets\\objects\\{item.Hash[..2]}\\{item.Hash}");
                                TaskItems.Add(new(native, localp, item.Size));
                            }
                            #endregion
                            #region Lib
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
                        }
                    }
                    catch (Exception ex)
                    {
                        LogWriteLine(ex.Message);
                        State = DownloadProgressState.Failed; return;
                    }
                }
                #endregion
                #region Download
                List<DownloadURI> urls = new List<DownloadURI>();
                for (int i = 0; i < TaskItems.Count; i++)
                {
                    TaskItems[i].NativeUrl = SourceReplacer.Replace(TaskItems[i].NativeUrl, sources);
                }
                var queue = new DownloadQueue(urls.ToArray());
                queue.ItemsUpdated += Queue_ItemsUpdated;
                queue.Download();
                #endregion
            }).Start();
            base.Start();
        }

        private void Queue_ItemsUpdated(object? sender, DownloadFile e)
        {
            var ti = TaskItems.Where(x => x.NativeUrl == e.Source.RemoteUri).ToArray()[0];
            ti.IsOver = true;
            ti.Downloaded = ti.Length;
        }
    }
}
