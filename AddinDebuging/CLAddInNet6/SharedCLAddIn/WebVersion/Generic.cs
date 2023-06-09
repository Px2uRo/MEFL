﻿using MEFL.Arguments;
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
        public override bool DirectDownload(FileInfo[] Javas,string dotMCPath,out IInstallPage page,out InstallArguments args)
        {
            var SubFolderString = Path.GetFileNameWithoutExtension(Url);
            page = new InstallPage(Javas,this,dotMCPath);
            args = InstallArguments.Empty;
            return false;
        }
    }
}
