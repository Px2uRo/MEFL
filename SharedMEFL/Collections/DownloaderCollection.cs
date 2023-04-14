using MEFL.APIData;
using MEFL.Contract;
using MEFL.InfoControls;
using MEFL.PageModelViews;
using MEFL.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;

namespace MEFL
{
    public class DownloadProgressCollection : ObservableCollection<DownloadProgress>
    {

        protected override void InsertItem(int index, DownloadProgress item)
        {
#if WPF
            DownloadingProgressPageModel.ModelView.ContentGrid.Children.Add(new Controls.DownloadProgressCard() { DataContext=item});
            base.InsertItem(index, item);
#endif
#if AVALONIA
            var block = new DownloadingProgressBlock(item);
            DownloadProcessPage.UI.ContentPanel.Children.Add(block);

            base.InsertItem(index, item);
#endif
        }

        internal string[] GetUsingFiles()
        {
            var res = new List<string>();
            foreach (var item in this)
            {
                if(item.GetUsingLocalFiles(out var paths))
                {
                    res.AddRange(paths);
                }
            }
            return res.ToArray();
        }

#if WPF
        protected override void RemoveItem(int index)
        {
            this[index].Close();
            this[index].Dispose();
            App.Current.Dispatcher.Invoke(() => {
                DownloadingProgressPageModel.ModelView.ContentGrid.Children.RemoveAt(index);
            });
            base.RemoveItem(index);
            if (this.Count == 0)
            {

            }
            GameRefresher.RefreshCurrect();
        }
#elif AVALONIA
        //TODO Avalonia 自我的搞法
#endif
    }
        public class DownloaderCollection : ObservableCollection<Contract.MEFLDownloader>
    {
        protected override void InsertItem(int index, MEFLDownloader item)
        {
            base.InsertItem(index, item);
            SettingPageModel.ModelView.Invoke(nameof(SettingPageModel.ModelView.Downloaders));
#if WPF
            SettingPageModel.ModelView.Invoke(nameof(SettingPageModel.ModelView.SelectedDownloaderString));
#endif
        }
        protected override void RemoveItem(int index)
        {
            if (APIModel.SelectedDownloader == this[index])
            {
                APIModel.SelectedDownloader = null;
            }
            this[index].Dispose();
            base.RemoveItem(index);
            if (Count > 0)
            {
                APIModel.SelectedDownloader = this[0];
            }
            SettingPageModel.ModelView.Invoke(nameof(SettingPageModel.ModelView.Downloaders));
#if WPF
            SettingPageModel.ModelView.Invoke(nameof(SettingPageModel.ModelView.SelectedDownloaderString));
#endif
        }
    }
}
