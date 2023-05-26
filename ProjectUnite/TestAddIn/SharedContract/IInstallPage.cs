using MEFL.Arguments;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Contract
{
    public interface IInstallPage:IDialogContent
    {
        public event EventHandler<InstallArguments> Solved;
        public LauncherWebVersionInfo Info { get; set; }
    }

    public interface IInstallContextMenuPage : IDialogContent
    {
        public event EventHandler<InstallProcess> Solved;
    }
}
