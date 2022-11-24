using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.CLAddIn.WebVersion
{
    public class GenericWebVersion : LauncherWebVersionInfo
    {
        public override DownloadProgress Download(MEFLDownloader downloader, string CurrectFolderPath, SettingArgs args, DownloadSource[] sources)
        {
            return downloader.CreateProgress(Url,CurrectFolderPath,sources);
        }
    }
}
