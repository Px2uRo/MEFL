using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if AVALONIA
using CLAddIn.Views;
#endif

namespace MEFL.CLAddIn.WebVersion
{
    public class GenericWebVersion : LauncherWebVersionInfo
    {
        public override bool Download(MEFLDownloader downloader, string dotMCFolder, SettingArgs args, DownloadSource[] sources, string[] usingLocalFiles,out IInstallPage page)
        {
            var SubFolderString = Path.GetFileNameWithoutExtension(Url);
            page = new InstallPage(Url, dotMCFolder, SubFolderString, downloader, sources, usingLocalFiles);
            return false;
        }
    }
}
