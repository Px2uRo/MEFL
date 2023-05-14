/*using CoreLaunching.JsonTemplates;
using MEFL.CLAddIn.Downloaders;
using MEFL.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MEFL.CLAddIn.CLDownding
{
    public class CLAddInDownloadingProgress : InstallProcess
    {
        List<CLWebFile> webFiles = new();
        List<CLWebFile> downloadingWebFiles = new();

        private void CLAddInDownloadingProgress_DownloadFinished(CLWebFile sender, long length)
        {
            DownloadedSize += length;
            DownloadedItems ++;
            var lst = webFiles.Where(x => x.IsDone == false || x.IsDownloading == false).ToList();
            if (lst.Count > 0)
            {
                var item = lst[0];
                var inde = downloadingWebFiles.IndexOf(sender);
                webFiles.Remove(sender);
                downloadingWebFiles[inde] = item;
                sender.Dispose();
                new DownThread(() =>
                {
                    item.DownloadFinished += CLAddInDownloadingProgress_DownloadFinished;
                    var tryed = item.TryDownload(out var local);
                    while (tryed)
                    {
                        if (item.TriedTimes >= 5)
                        {
                            break;
                        }
                        tryed = item.TryDownload(out var no);
                    }
                }).Start();
            }
        }

        public override void Start()
        {
            paused = false;
            new DownThread(() => {
                try
                {
                    var Key = NativeLocalPairs[0].NativeUrl;
                    var Value = NativeLocalPairs[0].LocalPath;
                    while (!Decided)
                    {
                        NativeLocalPairs.Clear();
                        var CLWeb = new CLWebFile(Key, Value);
                        if (CLWeb.TryDownload(out var localp))
                        {
                            if (Value.EndsWith(".json"))
                            {
                                var root = JsonConvert.DeserializeObject<Root>(System.IO.File.ReadAllText(Value));
                                if (root != null)
                                {
                                    GameJarPath = Path.Combine(versionPath, $"{Path.GetFileNameWithoutExtension(Value)}\\{Path.GetFileNameWithoutExtension(Value)}.jar");
                                    NativeLocalPairs.Add(new(SourceReplacer.Replace(root.Downloads.Client.Url, Sources), GameJarPath));
                                    TotalSize += root.Downloads.Client.Size;
                                    CurrectProgressIndex = "判断缺失的文件中";
                                    if (!Directory.Exists(Path.Combine(dotMCPath, "assets", "indexs")))
                                    {
                                        Directory.CreateDirectory(Path.Combine(dotMCPath, "assets", "indexs"));
                                        var clt = new WebClient();
                                        var address = SourceReplacer.Replace(root.AssetIndex.Url, Sources);
                                        clt.DownloadFile(address, Path.Combine(dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json"));
                                        clt.Dispose();
                                    }
                                    if (!System.IO.File.Exists(Path.Combine(dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json")))
                                    {
                                        var clt = new WebClient();
                                        var address = SourceReplacer.Replace(root.AssetIndex.Url, Sources);
                                        clt.DownloadFile(address, Path.Combine(dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json"));
                                        clt.Dispose();
                                    }
                                    var assets = JsonConvert.DeserializeObject<AssetsObject>(System.IO.File.ReadAllText(Path.Combine(dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json")));
                                    if (assets == null)
                                    {
                                        var clt = new WebClient();
                                        var address = SourceReplacer.Replace(root.AssetIndex.Url, Sources);
                                        clt.DownloadFile(address, Path.Combine(dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json"));
                                        clt.Dispose();
                                    }
                                    GC.SuppressFinalize(assets);
                                    assets = JsonConvert.DeserializeObject<AssetsObject>(System.IO.File.ReadAllText(Path.Combine(dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json")));
                                    if (assets == null)
                                    {
                                        State = DownloadProgressState.Failed;
                                    }
                                    else
                                    {
                                        foreach (var item in assets.Objects)
                                        {
                                            var objectPath = Path.Combine(dotMCPath, "assets", "objects", item.Hash.Substring(0, 2), item.Hash);
                                            if (!System.IO.File.Exists(objectPath))
                                            {
                                                var native = SourceReplacer.Replace($"http://resources.download.minecraft.net/{item.Hash.Substring(0, 2)}/{item.Hash}", Sources);
                                                var nlp = new NativeLocalPair(native, objectPath);
                                                NativeLocalPairs.Add(nlp);
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
                                            native = SourceReplacer.Replace(item.Downloads.Artifact.Url, Sources);
                                            local = Path.Combine(dotMCPath, "libraries", item.Downloads.Artifact.Path.Replace("/", "\\"));
                                        }
                                        if (item.Downloads.Classifiers != null)
                                        {
                                            for (int j = 0; j < item.Downloads.Classifiers.Count; j++)
                                            {
                                                var classifier = item.Downloads.Classifiers[j];
                                                var classnative = classifier.Item.Url;
                                                var classlocal = Path.Combine(dotMCPath, "libraries", classifier.Item.Path.Replace("/", "\\"));
                                                if (!System.IO.File.Exists(classlocal))
                                                {
                                                    classnative = SourceReplacer.Replace(classnative, Sources);
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
                                                native = SourceReplacer.Replace(native, Sources);
                                                NativeLocalPairs.Add(new(native, local));
                                                TotalSize += item.Downloads.Artifact.Size;
                                                TotalCount++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        var MyStr = JsonConvert.SerializeObject(NativeLocalPairs);
                        Decided = true;
                    }
                    for (int i = 0; i < NativeLocalPairs.Count; i++)
                    {
                        webFiles.Add(new(NativeLocalPairs[i].NativeUrl, NativeLocalPairs[i].LocalPath));
                    }
                    for (int i = 0; i < 64; i++)
                    {
                        downloadingWebFiles.Add(webFiles[i]);
                        webFiles[i].DownloadFinished += CLAddInDownloadingProgress_DownloadFinished;
                    }
                    foreach (var item in downloadingWebFiles)
                    {
                        new DownThread(() =>
                        {
                            var tryed = item.TryDownload(out var local);
                            while (tryed)
                            {
                                if (item.TriedTimes >= 5)
                                {
                                    break;
                                }
                                tryed = item.TryDownload(out var no);
                            }
                        }).Start();
                    }
                }
                catch (Exception ex)
                {

                }

            }).Start();
            base.Start();
        }

        DownloadSource[] Sources;
        List<bool> bools = new();
        string dotMCPath;
        string versionPath;
        string GameJarPath;
        //Directory.CreateDirectory(System.IO.Path.Combine(fp, "versions", NameBox.Text));
        public CLAddInDownloadingProgress(string nativeUrl, string loaclPath, string dotMCFolder, DownloadSource[] sources)
        {
            dotMCPath = dotMCFolder;
            CurrectProgressIndex = Path.GetFileName(loaclPath);
            versionPath = Path.Combine(dotMCFolder, "versions", Path.GetFileNameWithoutExtension(CurrectProgressIndex));
            GameJarPath = Path.Combine(versionPath, $"{Path.GetFileNameWithoutExtension(CurrectProgressIndex)}.jar");
            Sources = sources;
            nativeUrl = SourceReplacer.Replace(nativeUrl, sources);
            this.NativeLocalPairs = new() { new(nativeUrl, loaclPath) };

        }

        public CLAddInDownloadingProgress(List<NativeLocalPair> nativeLocalPairs, string dotMCFolder)
        {
            this.NativeLocalPairs = nativeLocalPairs;
        }


        public CLAddInDownloadingProgress(List<NativeLocalPair> nativeLocalPairs, string dotMCFolder, DownloadSource[] sources)
        {
            this.NativeLocalPairs = nativeLocalPairs;
        }
        bool paused;
        public override void Pause()
        {
            State = DownloadProgressState.Pauseing;
            paused = true;
        }
        public override void Cancel()
        {
            State = DownloadProgressState.Canceling;
            base.Cancel();
        }
        public override void Close()
        {
            base.Close();
        }
        bool Decided = false;


        int _SavedFileSizes;
        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (bools[bools.Count - 1] == false)
            {
                if (bools.Count - 1 == 0)
                {
                    if (bools[0] == false)
                    {
                        TotalSize = (int)e.TotalBytesToReceive;
                    }
                }
                bools[bools.Count - 1] = true;
            }
            DownloadedSize = _SavedFileSizes + (int)e.BytesReceived;
            if (e.ProgressPercentage == 100)
            {
                DownloadedItems++;
                _SavedFileSizes += (int)e.TotalBytesToReceive;
            }
        }

        public override void Continue()
        {
            State = DownloadProgressState.RetryingOrContiuning;
            paused = false;
        }
    }

    internal class CLWebFile:IDisposable
    {
        public bool IsCLDown = false;
		public delegate void DownloadSizeChangedEH(CLWebFile sender, long bytes);
		public event DownloadSizeChangedEH DownloadSizeChanged;
        public delegate void DownloadFinishedEH(CLWebFile sender,long length);
        public event DownloadFinishedEH DownloadFinished;
		private long _Downloaded;

		public long Downloaded
		{
			get { return _Downloaded; }
			set { _Downloaded = value; }
		}

		private long _Length;

		public long Length
		{
			get { return _Length; }
			set { _Length = value; }
		}

		private bool _IsDone;

		public bool IsDone
		{
			get { return _IsDone; }
			set { _IsDone = value; }
		}

        private bool _IsDownloading;

        public bool IsDownloading
        {
            get { return _IsDownloading; }
            set { _IsDownloading = value; }
        }


        public int TriedTimes = 0;
		public string LastExceptionMessage = string.Empty;
		private string Native;
		private string Local;
        private bool disposedValue;

        public bool TryDownload(out string local)
		{
			TriedTimes++;
            local = Local;
			Downloaded = 0;
            try
			{
				using(var res = HttpWebRequest.Create(Native).GetResponse())
				{
					Length = res.ContentLength;
				}
                Directory.CreateDirectory(local.Replace(Path.GetFileName(local),string.Empty));
				//if (Length > 100000)
				//{
    //                IsCLDown = true;
    //                //using (var client = new CoreLaunching.Downloader())
    //                using (var client = new CoreLaunching.Downloader())
    //                {
    //                    var delu = (int)(Length / 100000);
    //                    client.SpeedChanged += Client_SpeedChanged;
    //                    client.Finished += Client_Finished;
    //                    IsDownloading= true;
    //                    client.DirectDownload(Native,Local,ThreadNum:delu);
				//		client.Dispose();
				//	}
				//}
				//else
				//{
					using (var client = new WebClient())
					{
                        client.DownloadProgressChanged += Client_DownloadProgressChanged;
                        client.DownloadFileCompleted += Client_DownloadFileCompleted;
                        IsDownloading= true;
                        client.DownloadFileTaskAsync(Native, local).Wait();
                        client.Dispose();
                    }
                //}
                return true;
			}
			catch (Exception ex)
			{
				LastExceptionMessage= ex.Message;
                return false;
            }
		}

        private void Client_Finished(object sender)
        {
            Downloaded = Length;
            IsDownloading= false;
            if (DownloadSizeChanged != null)
            {
                DownloadSizeChanged.Invoke(this,Downloaded);
            }
            if (DownloadFinished != null)
            {
                DownloadFinished.Invoke(this,Length);
            }
        }

        private void Client_SpeedChanged(object sender)
        {
            if (DownloadSizeChanged != null)
            {
                Downloaded = (sender as CoreLaunching.Downloader).Downloaded;

                DownloadSizeChanged.Invoke(this, Downloaded);
            }

        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
			if (DownloadSizeChanged != null)
			{
				Downloaded = e.BytesReceived;

                DownloadSizeChanged.Invoke(this, Downloaded);
			}
        }

        private void Client_DownloadFileCompleted(object? sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            IsDownloading = false;
            Downloaded = Length;
			if (DownloadSizeChanged != null)
			{
				DownloadSizeChanged.Invoke(this, Downloaded);
			}
            if (DownloadFinished != null)
            {
                DownloadFinished.Invoke(this, Length);
            }
        }

        public CLWebFile(string native,string local)
		{
			Native= native;
			Local= local;
		}

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

				// TODO: 释放未托管的资源(未托管的对象)并重写终结器
				// TODO: 将大型字段设置为 null
				Downloaded = 0;
				Length= 0;
				IsDone= false;
				TriedTimes= 0;
				LastExceptionMessage = null;
				Native = null;
				Local = null;
                disposedValue = true;
				DownloadSizeChanged = null;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~WebFile()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public class CLAddInDownloader : MEFLDownloader
    {
        public override string Name => "[UNDER DEVLOPMENT]CLAddInDownloader";

        public override string Description => "CLAddIn 下载器";

        public override Version Version => new(1, 0, 0);

        public override object pubIcon => "CLAddIn";

        public override InstallProcess CreateProgress(string NativeUrl, string LoaclPath, DownloadSource[] sources, string dotMCFolder)
        {
            return new CLAddInDownloadingProgress(NativeUrl, LoaclPath, dotMCFolder,sources);
        }

        public override InstallProcess CreateProgress(List<NativeLocalPair> NativeLocalPairs, DownloadSource[] sources, string dotMCFolder)
        {
            return new CLAddInDownloadingProgress(NativeLocalPairs, dotMCFolder);
        }
    }

}
*/