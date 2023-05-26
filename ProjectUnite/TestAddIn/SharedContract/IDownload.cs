using MEFL.Arguments;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MEFL.Contract 
{

    public interface IDownload
    {
        /// <summary>
        /// 获取下载器
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public MEFLDownloader[] GetDownloaders(SettingArgs args);
        /// <summary>
        /// 获取下载源
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DownloadSource[] GetDownloadSources(SettingArgs args);

        /// <summary>
        /// 获取下载游戏页面的条目
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DownloadPageItemPair[] GetPairs(SettingArgs args);

        public LauncherWebVersionContext[] GetDataCotexts(LauncherWebVersionInfo baseInfo, FileInfo[] Javas, string dotMCPath);
    }
}