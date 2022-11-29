﻿using CoreLaunching.JsonTemplates;
using MEFL.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Controls;

namespace MEFL.CLAddIn.Downloaders
{
    public class NormalDownloadProgress : DownloadProgress
    {
        List<bool> bools = new();
        WebClient webClient;
        string dotMCPath;
        string versionPath;
        string GameJarPath;
        //Directory.CreateDirectory(System.IO.Path.Combine(fp, "versions", NameBox.Text));
        public NormalDownloadProgress(string nativeUrl, string loaclPath,string dotMCFolder)
        {
            dotMCPath = dotMCFolder;
            CurrectFile = Path.GetFileName( loaclPath);
            versionPath = Path.Combine(dotMCFolder, "versions", Path.GetFileNameWithoutExtension(CurrectFile));
            GameJarPath = Path.Combine(versionPath, $"{Path.GetFileNameWithoutExtension(CurrectFile)}.jar");
            this.NativeLocalPairs = new() { new(nativeUrl, loaclPath) };
        }


        public NormalDownloadProgress(List<NativeLocalPair> nativeLocalPairs,string dotMCFolder)
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
        public override void Start()
        {
            paused = false;
            new Thread(() => {
                for (int i = 0; i < this.NativeLocalPairs.Count; i++)
                {
                    if (State == DownloadProgressState.Canceling)
                    {
                        State = DownloadProgressState.Canceled;
                        break;
                    }
                    if (paused)
                    {
                        State = DownloadProgressState.Paused;
                    }
                    while (paused)
                    {
                        Thread.Sleep(100);
                        if(State == DownloadProgressState.RetryingOrContiuning)
                        {
                            paused = false;
                            State = DownloadProgressState.Downloading;
                        }
                    }
                    var Key = NativeLocalPairs[i].NativeUrl;
                    var Value = NativeLocalPairs[i].LoaclPath;
                    bools.Add(false);
                    CurrectFile = System.IO.Path.GetFileName(Key);
                    webClient = new();
                    webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                    webClient.Encoding = Encoding.UTF8;
                    try
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(Value)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(Value));
                        }
                        webClient.DownloadFileTaskAsync(Key, Value).Wait();
                    }
                    catch (System.Exception ex)
                    {
                        State = DownloadProgressState.Failed;
                    }
                    webClient.Dispose();
                    webClient = null;
                    while (!Decided)
                    {
                        if (Value.EndsWith(".json"))
                        {
                            var root = JsonConvert.DeserializeObject<Root>(System.IO.File.ReadAllText(Value));
                            if(root != null)
                            {
                                NativeLocalPairs.Add(new(root.Downloads.Client.Url, Path.Combine(versionPath, GameJarPath)));
                                TotalSize += root.Downloads.Client.Size;
                                CurrectFile = "判断缺失的文件中";
                                if(!Directory.Exists(Path.Combine(dotMCPath, "assets", "indexs")))
                                {
                                    Directory.CreateDirectory(Path.Combine(dotMCPath, "assets", "indexs"));
                                    var clt = new WebClient();
                                    clt.DownloadFile(root.AssetIndex.Url,Path.Combine(dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json"));
                                    clt.Dispose();
                                }
                                if (!System.IO.File.Exists(Path.Combine(dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json")))
                                {
                                    var clt = new WebClient();
                                    clt.DownloadFile(root.AssetIndex.Url,Path.Combine(dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json"));
                                    clt.Dispose();
                                }
                                var assets = JsonConvert.DeserializeObject<AssetsObject>(System.IO.File.ReadAllText(Path.Combine(dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json")));
                                if (assets == null)
                                {
                                    var clt = new WebClient();
                                    clt.DownloadFile(root.AssetIndex.Url, Path.Combine(dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json"));
                                    clt.Dispose();
                                }
                                GC.SuppressFinalize(assets);
                                assets = JsonConvert.DeserializeObject<AssetsObject>(System.IO.File.ReadAllText(Path.Combine(dotMCPath, "assets", "indexs", $"{root.AssetIndex.Id}.json")));
                                if(assets == null)
                                {
                                    State = DownloadProgressState.Failed;
                                }
                                else
                                {
                                    foreach (var item in assets.Objects)
                                    {
                                        var objectPath = Path.Combine(dotMCPath, "assets", "objects",item.Hash.Substring(0,2),item.Hash);
                                        if (!System.IO.File.Exists(objectPath))
                                        {
                                            NativeLocalPairs.Add(new($"http://resources.download.minecraft.net/{item.Hash.Substring(0, 2)}/{item.Hash}", objectPath));
                                            TotalSize += item.Size;
                                            TotalCount ++;
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
                                        local = Path.Combine(dotMCPath, "libraries", item.Downloads.Artifact.Path.Replace("/", "\\"));
                                    }
                                    if(item.Downloads.Classifiers!=null)
                                    {
                                        for (int j = 0; j < item.Downloads.Classifiers.Count; j++)
                                        {
                                            var classifier = item.Downloads.Classifiers[j];
                                            var classnative = classifier.Item.Url;
                                            var classlocal = Path.Combine(dotMCPath, "libraries", classifier.Item.Path.Replace("/", "\\"));
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
                                }
                            }
                        }
                        Decided = true;
                    }
                }
                State = DownloadProgressState.Finished;
            }).Start();
            base.Start();
        }

        int _SavedFileSizes;
        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if(bools[bools.Count - 1] == false)
            {
                if(bools.Count - 1 == 0)
                {
                    if( bools[0] == false)
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
}