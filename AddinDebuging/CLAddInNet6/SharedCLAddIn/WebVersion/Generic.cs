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
        public override Contract.LauncherProgressResult Download(MEFLDownloader downloader, string dotMCFolder, SettingArgs args, DownloadSource[] sources)
        {
            var SubFolderString = Path.GetFileNameWithoutExtension(Url);
            if (!Directory.Exists(Path.Combine(dotMCFolder, "versions", SubFolderString)))
            {
                Directory.CreateDirectory(Path.Combine(dotMCFolder, "versions", SubFolderString));
                return new(false, null, downloader.CreateProgress(Url, Path.Combine(dotMCFolder, "versions", SubFolderString, $"{SubFolderString}.json"), sources, dotMCFolder));
            }
            else
            {
                return new(true, new SolveDupNamePage(Url, dotMCFolder, SubFolderString), downloader.CreateProgress(Url, dotMCFolder, sources, dotMCFolder));
            }
        }
    }
}
