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
        public override Contract.LauncherProgressResult Download(MEFLDownloader downloader, string CurrectFolderPath, SettingArgs args, DownloadSource[] sources)
        {
            var SubFolderString = Path.GetFileNameWithoutExtension(Url);
            if (!Directory.Exists( Path.Combine(CurrectFolderPath, "versions", SubFolderString)))
            {
                Directory.CreateDirectory(Path.Combine(CurrectFolderPath, "versions", SubFolderString));
                return new(false, null, downloader.CreateProgress(Url, CurrectFolderPath, sources));
            }
            else
            {
                return new(true, new SolveDupNamePage(), downloader.CreateProgress(Url, CurrectFolderPath, sources));
            }
        }
    }
}
