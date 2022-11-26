using CoreLaunching.JsonTemplates;
using MEFL.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        string versionPath;
        string GameJarPath;
        //Directory.CreateDirectory(System.IO.Path.Combine(fp, "versions", NameBox.Text));
        public NormalDownloadProgress(string nativeUrl, string loaclPath,string dotMCFolder)
        {
            CurrectFile = Path.GetFileName( loaclPath);
            versionPath = Path.Combine(dotMCFolder, "versions", Path.GetFileNameWithoutExtension(CurrectFile));
            GameJarPath = Path.Combine(versionPath, $"{Path.GetFileNameWithoutExtension(CurrectFile)}.jar");
            this.NativeLocalPairs = new() { { nativeUrl, loaclPath } };
        }


        public NormalDownloadProgress(Dictionary<string, string> nativeLocalPairs,string dotMCFolder)
        {
            this.NativeLocalPairs = nativeLocalPairs;
        }
        bool paused;
        public override void Pause()
        {
            paused = true;
            base.Pause();
        }
        public override void Cancel()
        {
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
                    var Key = NativeLocalPairs.ToList()[i].Key.ToString();
                    var Value = NativeLocalPairs.ToList()[i].Value.ToString();
                    bools.Add(false);
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
                                NativeLocalPairs.Add(root.Downloads.Client.Url,Path.Combine(versionPath,GameJarPath));
                            }
                        }
                        Decided = true;
                    }
                }
            }).Start();
            base.Start();
        }

        int _SavedFileSizes;
        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if(bools[bools.Count - 1] == false)
            {
                bools[bools.Count - 1] = true;
                TotalSize += (int)e.TotalBytesToReceive;
            }
            DownloadedSize = _SavedFileSizes + (int)e.BytesReceived;
            if (e.ProgressPercentage == 100)
            {
                _SavedFileSizes = (int)e.TotalBytesToReceive;
            }
        }

        public override void Continue()
        {
            paused = false;
            Start();
        }
    }
}