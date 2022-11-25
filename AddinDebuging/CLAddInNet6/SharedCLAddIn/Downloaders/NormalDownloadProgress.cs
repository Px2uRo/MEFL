using MEFL.Contract;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Controls;

namespace MEFL.CLAddIn.Downloaders
{
    internal class NormalDownloadProgress : DownloadProgress
    {
        public NormalDownloadProgress(string nativeUrl, string loaclPath)
        {
            CurrectFile = nativeUrl;
            this.NativeLocalPairs = new () { { nativeUrl, loaclPath } };
        }


        public NormalDownloadProgress(Dictionary<string, string> nativeLocalPairs)
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
        public override void Start()
        {
            paused = false;
            base.Start();
        }
        public override void Continue()
        {
            paused = false;
            Start();
        }
    }
}