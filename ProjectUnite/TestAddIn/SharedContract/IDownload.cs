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

        /// <summary>
        /// ��ȡ�汾���صĸ����
        /// </summary>
        /// <param name="baseInfo">������Ϣ</param>
        /// <param name="Javas">Java �б�</param>
        /// <param name="dotMCPath">.minecraft �ļ���λ��</param>
        /// <returns>�汾���صĸ�����</returns>
        public LauncherWebVersionContext[] GetDataContexts(LauncherWebVersionInfo baseInfo, FileInfo[] Javas, string dotMCPath);
    

    }
}