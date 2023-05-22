using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.CLAddIn
{
    internal class WDCServer : LauncherWebVersionContext
    {
        public override string Name => "Server";

        public override bool DirectDownload(string url, FileInfo[] Javas, string dotMCPath, out IInstallPage page, out InstallArguments args)
        {
            throw new NotImplementedException();
        }
    }
}
