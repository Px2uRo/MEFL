using MEFL.Arguments;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MEFL.Contract 
{

    public interface IDownload
    {
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public MEFLDownloader[] GetDownloaders(SettingArgs args);
        /// <summary>
        /// ��ȡ����Դ
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DownloadSource[] GetDownloadSources(SettingArgs args);

        /// <summary>
        /// ��ȡ������Ϸҳ�����Ŀ
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DownloadPageItemPair[] GetPairs(SettingArgs args);

        public LauncherWebVersionContext[] GetDataCotexts(LauncherWebVersionInfo baseInfo, FileInfo[] Javas, string dotMCPath);
    }
}