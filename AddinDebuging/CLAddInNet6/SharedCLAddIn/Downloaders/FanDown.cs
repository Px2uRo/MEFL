using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using CoreLaunching;
using CoreLaunching.JsonTemplates;
using Newtonsoft.Json;
using MEFL.Contract.Helpers;
using MEFL.Contract.Web;

namespace MEFL.CLAddIn.Downloaders
{
    #region ThankstoFanbal


    #endregion



    public class FandownProgress : DownloadProgress
    {
        private MEFL.Contract.Web.DownloadFilePool _pool;
        private string _dotMCPath;
        private string _versionPath;
        private string _gameJarPath;
        private List<DownloadSource> _sources;

        private bool _paused;
        private bool _decided;

        private ManifestFile _manifestFile; // 最开始的远端配置结构。


        //Directory.CreateDirectory(System.IO.Path.Combine(fp, "versions", NameBox.Text));
        public FandownProgress(ManifestFile manifestFile, string dotMCFolder, IEnumerable<DownloadSource> sources)
        {
            _manifestFile = manifestFile;
            _manifestFile.OnTaskCompleted += _manifestFile_OnTaskCompleted;

            //_sources = new List<DownloadSource>(sources);
            //_dotMCPath = dotMCFolder;
            //_versionPath = Path.Combine(dotMCFolder, "versions", Path.GetFileNameWithoutExtension(CurrectFile));
            //_gameJarPath = Path.Combine(_versionPath, $"{Path.GetFileNameWithoutExtension(CurrectFile)}.jar");
        }

        private void _manifestFile_OnTaskCompleted(object? sender, EventArgs e)
        {
            DownloadItems();
        }

        public override void Start()
        {
            if (_manifestFile.State == DownloadFileState.Ready)
            {
                _manifestFile.Download();
            }
            else
            {
                DownloadItems();
            }
        }

        ///<summary>下载多项。</summary>
        private void DownloadItems()
        {
            _pool = new DownloadFilePool(_manifestFile.NativeLocalPairs);

            _pool.OnProgressUpdated += _pool_OnProgressUpdated;
            _pool.OnDownloadFailed += _pool_OnDownloadFailed;
            _pool.OnDownloadAllCompleted += _pool_OnDownloadAllCompleted;
            _pool.OnBytesAdd += _pool_OnBytesAdd;
            _pool.StartDownload();
        }

        private void _pool_OnBytesAdd(object? sender, long e)
        {
            Console.WriteLine($"OnBytesAdd {e}");
        }

        private void _pool_OnDownloadAllCompleted(object? sender, EventArgs e)
        {
            Console.WriteLine($"OnDownloadAllCompleted {e}");
        }

        private void _pool_OnDownloadFailed(object? sender, DownloadFile e)
        {
            Console.WriteLine($"_pool_OnDownloadFailed {e}");
        }

        private void _pool_OnProgressUpdated(object? sender, DownloadFileProgressArgument e)
        {
            Console.WriteLine($"OnProgressUpdated {e}");
        }

        private void TaskFunction()
        {
            while (!_decided)
            {
                var Value = NativeLocalPairs[0].LocalPath;
                Directory.CreateDirectory(Value.Replace(Path.GetFileName(Value), string.Empty));

                if (NativeLocalPairs.Count > 1)
                {
                    Console.WriteLine("BREAK");
                    break;
                }

                new WebClient().DownloadFile(NativeLocalPairs[0].NativeUrl, Value);

                NativeLocalPairs.Clear();
                if (Value.EndsWith(".json"))
                {
                    var root = JsonConvert.DeserializeObject<Root>(System.IO.File.ReadAllText(Value));
                    if (root != null)
                    {
                        NativeLocalPairs.Add(new(root.Downloads.Client.Url, Path.Combine(_versionPath, _gameJarPath)));
                        TotalSize += root.Downloads.Client.Size;
                        CurrectFile = "判断缺失的文件中";
                        if (!Directory.Exists(Path.Combine(_dotMCPath, "assets", "indexs")))
                        {
                            Directory.CreateDirectory(Path.Combine(_dotMCPath, "assets", "indexs"));
                            var clt = new WebClient();
                            clt.DownloadFile(root.AssetIndex.Url, Path.Combine(_dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json"));
                            clt.Dispose();
                        }
                        if (!System.IO.File.Exists(Path.Combine(_dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json")))
                        {
                            var clt = new WebClient();
                            clt.DownloadFile(root.AssetIndex.Url, Path.Combine(_dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json"));
                            clt.Dispose();
                        }
                        var assets = JsonConvert.DeserializeObject<AssetsObject>(System.IO.File.ReadAllText(Path.Combine(_dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json")));
                        if (assets == null)
                        {
                            var clt = new WebClient();
                            clt.DownloadFile(root.AssetIndex.Url, Path.Combine(_dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json"));
                            clt.Dispose();
                        }
                        GC.SuppressFinalize(assets);
                        assets = JsonConvert.DeserializeObject<AssetsObject>(System.IO.File.ReadAllText(Path.Combine(_dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json")));
                        if (assets == null)
                        {
                            State = DownloadProgressState.Failed;
                        }
                        else
                        {
                            foreach (var item in assets.Objects)
                            {
                                var objectPath = Path.Combine(_dotMCPath, "assets", "objects", item.Hash.Substring(0, 2), item.Hash);
                                if (!System.IO.File.Exists(objectPath))
                                {
                                    NativeLocalPairs.Add(new($"http://resources.download.minecraft.net/{item.Hash.Substring(0, 2)}/{item.Hash}", objectPath));
                                    TotalSize += item.Size;
                                    TotalCount++;
                                }
                            }
                        }
                        foreach (var item in root.Libraries)
                        {
                            var native = string.Empty;
                            var local = string.Empty;
                            if (item.Downloads.Artifact != null)
                            {
                                native = item.Downloads.Artifact.Url;
                                local = Path.Combine(_dotMCPath, "libraries", item.Downloads.Artifact.Path.Replace("/", "\\"));
                            }
                            if (item.Downloads.Classifiers != null)
                            {
                                for (int j = 0; j < item.Downloads.Classifiers.Count; j++)
                                {
                                    var classifier = item.Downloads.Classifiers[j];
                                    var classnative = classifier.Item.Url;
                                    var classlocal = Path.Combine(_dotMCPath, "libraries", classifier.Item.Path.Replace("/", "\\"));
                                    if (!System.IO.File.Exists(classlocal))
                                    {
                                        NativeLocalPairs.Add(new(classnative, classlocal));
                                        TotalSize += classifier.Item.Size;
                                        TotalCount++;
                                    }
                                }
                            }
                            if (!(string.IsNullOrEmpty(native) && string.IsNullOrEmpty(local)))
                            {
                                if (!System.IO.File.Exists(local))
                                {
                                    NativeLocalPairs.Add(new(native, local));
                                    TotalSize += item.Downloads.Artifact.Size;
                                    TotalCount++;
                                }
                            }
                            foreach (var my in this.NativeLocalPairs)
                            {
                                my.NativeUrl = SourceReplacer.Replace(my.NativeUrl, new List<DownloadSource>(_sources).ToArray());
                            }
                        }
                    }
                }
                _decided = true;
            }
            _pool = new MEFL.Contract.Web.DownloadFilePool(NativeLocalPairs);
            _pool.OnProgressUpdated += POOL_OnProgressUpdated;
            _pool.OnDownloadFailed += POOL_OnDownloadFailed;
            _pool.OnDownloadAllCompleted += POOL_OnDownloadAllCompleted;
            _pool.OnBytesAdd += POOL_OnBytesAdd;
            _pool.StartDownload();
        }

        private void POOL_OnBytesAdd(object? sender, long e)
        {
            DownloadedSize = DownloadedSize + e;
        }

        private void POOL_OnDownloadAllCompleted(object? sender, EventArgs e)
        {
            State = DownloadProgressState.Finished;
        }

        private void POOL_OnDownloadFailed(object? sender, MEFL.Contract.Web.DownloadFile e)
        {
            DownloadedItems++;
            LogWriteLine($"{Path.GetFileName(e.Source.NativeUrl)}:{e.ErrorInfo}");
        }

        private void POOL_OnProgressUpdated(object? sender, MEFL.Contract.Web.DownloadFileProgressArgument e)
        {
            DownloadedItems = e.CompletedFileCount;
        }
    }
}
