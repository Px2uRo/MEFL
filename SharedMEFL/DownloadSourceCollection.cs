using MEFL.Contract;
using System.Collections.ObjectModel;

namespace MEFL.APIData
{
    public class DownloadSourceCollection:ObservableCollection<DownloadSource>
    {
        protected override void InsertItem(int index, DownloadSource item)
        {

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            this[index].Dispose();
            base.RemoveItem(index);
            
        }
    }
}