using MEFL.Arguments;
using System.Collections.ObjectModel;

namespace MEFL.Contract 
{

    public interface IDownload
    {
        public Downloader[] GetDownloaders(SettingArgs args);
        public DownloadSource[] GetDownloadSources(SettingArgs args);
        public DownloadPageItemPair[] GetPairs(SettingArgs args);
    }
}