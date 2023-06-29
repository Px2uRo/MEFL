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

        /// <summary>
        /// 获取版本下载的附加项。
        /// </summary>
        /// <param name="baseInfo">基本信息</param>
        /// <param name="Javas">Java 列表</param>
        /// <param name="dotMCPath">.minecraft 文件夹位置</param>
        /// <returns>版本下载的附加项</returns>
        public LauncherWebVersionContext[] GetDataContexts(LauncherWebVersionInfo baseInfo, FileInfo[] Javas, string dotMCPath);
    

    }
}