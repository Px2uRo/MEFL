using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MEFL.CLAddIn.WebVersion
{
    public class GenericWebVersion : LauncherWebVersionInfo
    {
        public override Contract.InstallProgressInput Download(MEFLDownloader downloader, string dotMCFolder, SettingArgs args, DownloadSource[] sources, string[] usingLocalFiles)
        {
            var SubFolderString = Path.GetFileNameWithoutExtension(Url);
            return new(true, new InstallPage(Url, dotMCFolder, SubFolderString, downloader, sources,usingLocalFiles), null);
        }
    }
}
