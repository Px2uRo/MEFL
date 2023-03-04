using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace MEFL.Contract
{
    public class Advanced
    {
        private static MEFLDownloader selectedDownloader { get; set; }
        [Obsolete("别用，不要用，不是给你用的")]
        public static void SetSelectedDownloader(MEFLDownloader value)
        {
            selectedDownloader= value;
        }
        public static MEFLDownloader GetSelectedDownloader()
        {
            return selectedDownloader;
        }
        [Obsolete("别用，不要用，不是给你用的")]
        public static EventHandler<DownloadProgress>? OnDownloadProgressCreated;
        public static void AddProgress(DownloadProgress downloadProgress)
        {
            OnDownloadProgressCreated?.Invoke(null,downloadProgress);
        }

        private static DownloadSource[] selectedSources { get; set; }
        [Obsolete("别用，不要用，不是给你用的")]
        public static void SetSelectedSources(DownloadSource[] value)
        {
            selectedSources = value;
        }
        public static DownloadSource[] GetSelectedSources() => selectedSources;
    }


}
