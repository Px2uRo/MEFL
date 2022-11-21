using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace MEFL
{
    internal class DownloaderCollection : ObservableCollection<Contract.MEFLDownloader>
    {
        protected override void RemoveItem(int index)
        {
            this[index].Dispose();
            base.RemoveItem(index);
        }
    }
}
