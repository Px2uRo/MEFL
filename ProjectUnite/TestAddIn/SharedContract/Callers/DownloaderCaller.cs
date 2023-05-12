using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Callers
{
    public static class DownloaderCaller
    {
        public static event EventHandler<DownloadSingleArgs> SingleCallerEvent;
        public static void CallSingle(string native,string local) 
        {
            SingleCallerEvent?.Invoke(null,new DownloadSingleArgs(native,local));
        }
    }

    public class DownloadSingleArgs:EventArgs
    {
        public string Native { get; private set; }
        public string Local { get; set; }
        public DownloadSingleArgs(string native, string local)
        {
            Native = native;
            Local = local;
        }
    }
}
