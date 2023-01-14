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
using MEFL.Contrat.Helpers;

namespace MEFL.CLAddIn.Downloaders
{
    #region ThankstoFanbal

    public enum DownloadFileState
    {
        ///<summary>完全没有进入下载。</summary>
        Ready,

        ///<summary>下载中</summary>
        Downloading,

        ///<summary>超时。</summary>
        DownloadOutTime,

        ///<summary>下载失败。</summary>
        DownloadFailed,

        ///<summary>下载成功。</summary>
        DownloadSucessed,

        ///<summary>下载虽然成功，但是本地文件找不到了。可能被删掉了。</summary>
        DownloadSuceessedButLocalFileMissed,
    }

    class DownloadFileProgressArgument : EventArgs
    {
        public readonly int TotalFileCount;
        public readonly int CompletedFileCount;

        public DownloadFileProgressArgument(int totalFileCount, int completedFileCount)
        {
            TotalFileCount = totalFileCount;
            CompletedFileCount = completedFileCount;
        }

        public override string ToString()
        {
            return $"{CompletedFileCount}/{TotalFileCount}";
        }
    }

    internal class DownloadFilePool
    {
        ///<summary>最大线程数。</summary>
        public int MAX_THREAD_COUNT = 64;


        public event EventHandler OnDownloadAllCompleted;
        public event EventHandler<DownloadFileProgressArgument> OnProgressUpdated;
        public event EventHandler<DownloadFile> OnDownloadFailed;
        public event EventHandler<long> OnBytesAdd;

        private ConcurrentQueue<DownloadFile> _readyQueue;
        private ConcurrentQueue<DownloadFile> _completedQueue;
        private string _targetDir;
        private int _totalCount;

        public DownloadFilePool(IEnumerable<NativeLocalPair> pairs)
        {

            _targetDir = AppDomain.CurrentDomain.BaseDirectory;

            _readyQueue = new ConcurrentQueue<DownloadFile>();
            foreach (var item in pairs)
            {
                var newFile = new DownloadFile(item);
                newFile.Source.LoaclPath = newFile.Source.LoaclPath.Replace("${请Ctrl+H替换}", _targetDir);
                newFile.OnBytesAdd += NewFile_OnBytesAdd;
                newFile.OnDownloadFailed += NewFile_OnDownloadFailed;
                newFile.OnTaskCompleted += NewFile_OnTaskCompleted;
                _readyQueue.Enqueue(newFile);
            }
            _totalCount = _readyQueue.Count;
            _completedQueue = new ConcurrentQueue<DownloadFile>();
        }

        private void NewFile_OnDownloadFailed(object? sender, string e)
        {
            OnDownloadFailed?.Invoke(this, (DownloadFile)sender);
        }

        private void NewFile_OnBytesAdd(object? sender, long e)
        {
            OnBytesAdd?.Invoke(this, e);
        }

        public void StartDownload()
        {
            for (int i = 0; i < MAX_THREAD_COUNT; i++)
            {
                ContinueDownload();
            }
        }


        private void NewFile_OnTaskCompleted(object? sender, EventArgs e)
        {
            _completedQueue.Enqueue((DownloadFile)sender);
            if (((DownloadFile)sender).State == DownloadFileState.DownloadFailed)
            {
                OnDownloadFailed?.Invoke(this, (DownloadFile)sender);
            }
            ContinueDownload();
            OnProgressUpdated?.Invoke(this, new DownloadFileProgressArgument(_totalCount, _completedQueue.Count));
        }

        private void ContinueDownload()
        {
            if (_completedQueue.Count == _totalCount) OnDownloadAllCompleted?.Invoke(this, EventArgs.Empty);
            if (_readyQueue.TryDequeue(out var unreadyFile))
            {
                unreadyFile.Download();
            }
        }


    }

    internal class DownloadFile
    {
        private CancellationTokenSource _cancelSource;

        public DownloadFileState State;

        public string ErrorInfo;

        public MEFL.Contract.NativeLocalPair Source;
        public event EventHandler<long> OnBytesAdd;
        public event EventHandler<string> OnDownloadFailed;

        public event EventHandler OnTaskCompleted;

        public DownloadFile(MEFL.Contract.NativeLocalPair source)
        {
            _cancelSource = new CancellationTokenSource();
            Source = source;
            State = DownloadFileState.Ready;
        }

        public async void Download()
        {
            var task = new Task(() =>
            {
                State = DownloadFileState.Downloading;
                try
                {
                    var httpRequest = WebRequest.Create(Source.NativeUrl);
                    httpRequest.Method = "GET";
                    httpRequest.ContentType = "application/x-www-form-urlencoded";

                    httpRequest.Timeout = 30000; // 半分钟。

                    var httpResponse = httpRequest.GetResponse();


                    using (var stream = httpResponse.GetResponseStream())
                    {
                        var parentRoot = Path.GetDirectoryName(Source.LoaclPath);
                        FileSystemHelper.CreateFolder(parentRoot);
                        using (FileStream fs = new FileStream(Source.LoaclPath, FileMode.Create))
                        {
                            var cancelToken = new CancellationTokenSource();
                            Task.Run(() =>
                            {
                                long lastLeng = 0;
                                while (cancelToken.IsCancellationRequested == false)
                                {
                                    Thread.Sleep(2500);
                                    if (cancelToken.IsCancellationRequested == false && fs.CanWrite)
                                    {
#if DEBUG
                                        Debug.WriteLine($"{Source.NativeUrl} {fs.Length / 1024}");
#endif
                                        OnBytesAdd?.Invoke(this, fs.Length - lastLeng);
                                    }
                                }
                            });
                            stream.CopyTo(fs);
                            cancelToken.Cancel();
                        }
                    }

                    State = DownloadFileState.DownloadSucessed;
                    OnTaskCompleted?.Invoke(this, EventArgs.Empty);

                }
                catch (Exception ex)
                {

                    Debug.WriteLine($"下载失败{Source.NativeUrl}，{ex.Message}");
                    State = DownloadFileState.DownloadFailed;
                    ErrorInfo = ex.Message;
                    OnDownloadFailed?.Invoke(this, ErrorInfo);
                    OnTaskCompleted?.Invoke(this, EventArgs.Empty);
                }

            });
            task.Start();

        }

        public override string ToString()
        {
            return $"remote uri: {Source.NativeUrl}, local uri: {Source.LoaclPath}";
        }

    }

    #endregion

    ///<summary>来自fanbal的下载器。</summary>
    internal class Fandown : MEFLDownloader
    {
        public override string Name => "FanDown";

        public override string Description => "多线程并发下载器";

        public override Version Version => new(1, 0, 0);

        public override object Icon => "FanDown";

        public override DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, IEnumerable<DownloadSource> sources, string dotMCFolder)
        {
            return new FandownProgress(NativeUrl, LoaclPath, dotMCFolder, sources);
        }

        public override DownloadProgress CreateProgress(IEnumerable<NativeLocalPair> NativeLocalPairs, IEnumerable<DownloadSource> sources, string dotMCFolder)
        {
            return new FandownProgress(NativeLocalPairs, dotMCFolder, sources);
        }

        public override DownloadProgress CreateProgress(NativeLocalPair nativeLocalPair, IEnumerable<DownloadSource> sources, string dotMCFolder)
        {
            return new FandownProgress(new[] { nativeLocalPair }, dotMCFolder, sources);
        }
    }

    public class FandownProgress : DownloadProgress
    {
        private DownloadFilePool _pool;
        private string _dotMCPath;
        private string _versionPath;
        private string _gameJarPath;
        private List<DownloadSource> _sources;

        private bool _paused;
        private bool _decided;

        //Directory.CreateDirectory(System.IO.Path.Combine(fp, "versions", NameBox.Text));
        public FandownProgress(string nativeUrl, string loaclPath, string dotMCFolder, IEnumerable<DownloadSource> sources)
        {
            _sources = new List<DownloadSource>(sources);
            _dotMCPath = dotMCFolder;
            CurrectFile = Path.GetFileName(loaclPath);
            _versionPath = Path.Combine(dotMCFolder, "versions", Path.GetFileNameWithoutExtension(CurrectFile));
            _gameJarPath = Path.Combine(_versionPath, $"{Path.GetFileNameWithoutExtension(CurrectFile)}.jar");
            NativeLocalPairs = new() { new(nativeUrl, loaclPath) };
        }


        public FandownProgress(IEnumerable<NativeLocalPair> nativeLocalPairs, string dotMCFolder, IEnumerable<DownloadSource> sources)
        {
            if (sources != null)
                _sources = new List<DownloadSource>(sources);

            NativeLocalPairs = new List<NativeLocalPair>(nativeLocalPairs);
        }


        public override void Start()
        {
            base.Start();
            _paused = false;
            new Task(() =>
            {
                try
                {
                    TaskFunction();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }).Start();
        }


        private void TaskFunction()
        {
            while (!_decided)
            {
                var Value = NativeLocalPairs[0].LoaclPath;
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
            _pool = new DownloadFilePool(NativeLocalPairs);
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

        private void POOL_OnDownloadFailed(object? sender, DownloadFile e)
        {
            DownloadedItems++;
            LogWriteLine($"{Path.GetFileName(e.Source.NativeUrl)}:{e.ErrorInfo}");
        }

        private void POOL_OnProgressUpdated(object? sender, DownloadFileProgressArgument e)
        {
            DownloadedItems = e.CompletedFileCount;
        }
    }
}
