using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace MEFL
{
    public class DownloadeProgressCollection : ObservableCollection<DownloadProgress>
    {
        protected override void RemoveItem(int index)
        {
            this[index].Close();
            this[index].Dispose();
            base.RemoveItem(index);
        }
    }
    internal class DownloaderCollection : ObservableCollection<Contract.MEFLDownloader>
    {
        protected override void RemoveItem(int index)
        {
            this[index].Dispose();
            base.RemoveItem(index);
        }
    }
}
