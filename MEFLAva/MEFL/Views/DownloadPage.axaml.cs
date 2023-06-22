using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData;
using MEFL.APIData;
using MEFL.Contract;
using MEFL.InfoControls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace MEFL.Views
{
    public partial class DownloadPage : UserControl
    {
        public static DownloadPage UI;
        public static ObservableCollection<TabItem> TabS;
        public DownloadPage()
        {
            InitializeComponent();
        }

        static DownloadPage()
        {
            UI = new DownloadPage();
            TabS = new ObservableCollection<TabItem>();
            UI.TabC.Items = TabS;
        }
        public static void AddPairs(DownloadPageItemPair[] pairs)
        {
            foreach (var pair in pairs)
            {
                var cont = new DownloadsTabItemContent(pair.Contents);
                pair.RefreshCompeted -= Pair_RefreshCompeted;
                pair.RefreshCompeted += Pair_RefreshCompeted;
                new Thread(() => {
                    while (APIModel.SettingConfig == null)
                    {

                    }
                    pair.WebRefresh(APIModel.SettingConfig.TempFolderPath);
                    pair.RefreshList(APIModel.SettingConfig.TempFolderPath);
                }).Start();
                var NewItem = new TabItem() { Header = pair.Title,Tag = pair.AddInGuid,Content =  cont};
                TabS.Add(NewItem);
            }
        }

        private static void Pair_RefreshCompeted(object? sender, System.EventArgs e)
        {
            var sen = (DownloadPageItemPair)sender;
            Dispatcher.UIThread.InvokeAsync(() => 
            {
                var lq = TabS.Where((x) => x.Tag.ToString() == sen.AddInGuid).ToArray();
                var lq2 = lq.Where((x) => x.Header.ToString() == sen.Title).ToArray();
                if (lq2.Length > 0)
                {
                    (lq2[0].Content as DownloadsTabItemContent).ReFresh(sen.Contents);
                    (lq2[0].Content as DownloadsTabItemContent).ShowInfo = false;
                }
            });
        }

        public static void RemoveTabItems(TabItem[] items)
        {
            TabS.RemoveMany(items);
        }
    }
}
