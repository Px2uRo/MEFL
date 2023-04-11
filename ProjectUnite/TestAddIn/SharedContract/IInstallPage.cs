using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Contract
{
    public interface IInstallPage
    {
        public event EventHandler<DownloadProgress> Solved;
    }
}
