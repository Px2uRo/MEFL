using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MEFL.Contract
{
    public interface IDownloadPage
    {
        public ObservableCollection<DownloadPageItemPair> GetPairs();
    }

    public class DownloadPageItemPair
    {
        public delegate void RefreshBeihvior(object sender,string tmpFolderPath);
        public event RefreshBeihvior? RefreshEvent;
        public string Title { get; private set; }
        public string Tag { get; set; }
        public List<LauncherWebVersionInfo> Items { get; set; }
        public DownloadPageItemPair(string title, List<LauncherWebVersionInfo> items,string tag)
        {
            Title = title;
            Items = items;
            Tag = tag;
        }
        public void InvokeRefreshEvent(object sender, string tmpFolderPath)
        {
            RefreshEvent.Invoke(sender,tmpFolderPath);
        }


    }

    public class LauncherWebVersionInfo
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }

}
