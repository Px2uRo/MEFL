using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using CoreLaunching.DownloadAPIs.Forge;
using CoreLaunching.Forge;
using CoreLaunching.MicrosoftAuth;
using MEFL.Callers;
using MEFL.CLAddIn;
using MEFL.Contract;
using System.Collections.ObjectModel;
using System.Net;

namespace CLAddIn.Views
{
    public partial class ResourceFinder : UserControl
    {
        protected override Size ArrangeOverride(Size finalSize)
        {
            var b = base.ArrangeOverride(finalSize);
            HotScrl.Height = b.Height - 200;
            return b;
        }
        public ResourceFinder()
        {
            InitializeComponent();
            CancelDetailBtn.Click += CancelDetailBtn_Click;
            OtherIndexs.SelectionChanged += OtherIndexs_SelectionChanged;
            LoadHot();
        }

        ForgeModInfo _currectInfo;
        LatestFilesIndex[] indexes;
        private void OtherIndexs_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var s = sender as ComboBox;
            if(s.SelectedIndex != -1)
            {
                var item = s.SelectedItem;
                Others.Children.Clear();
                var others = indexes.Where(x => x.GameVersion == item.ToString());
                foreach (var ite in others)
                {
                    var child = new ForgeFileItem(_currectInfo.Id, ite);
                    Others.Children.Add(child);
                }
            }
        }

        private void CancelDetailBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ListsPage.IsVisible = true;
            DetailStack.IsVisible = false;
        }

        private void LoadHot()
        {

            new Thread(() =>
            {
                var can = ForgeResourceFinder.GetFeatured(out var res, out var e);
                if (can)
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        HotInfoBD.IsVisible = false;
                        foreach (var item in res.Data.Popular)
                        {
                            var child = new ResourcesDownloadItem(item);
                            child.RootBtn.Click += RootBtn_Click;
                            HotList.Children.Add(child);
                        }
                    });
                }
                else
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        HotInfoTB.Text = e;
                    });
                }
            }).Start();

        }

        private void RootBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ListsPage.IsVisible = false;
            DetailStack.IsVisible= true;
            LoadDetail((sender as Control).Parent as ResourcesDownloadItem);
        }

        ObservableCollection<String> versions = new ObservableCollection<String>();
        private void LoadDetail(ResourcesDownloadItem control)
        {
            _currectInfo = control.DataContext as ForgeModInfo;
            DetailNameTB.Text = _currectInfo.Name;
            DetailDesTB.Text = _currectInfo.Summary;
            var versions = new List<Version>();
            DetailSupportVersionTB.Text = control.SupportVersionTB.Text;
            DetailDownloadCount.Text = _currectInfo.DownloadCounts;
            DetailAuthorsStack.Children.Clear();
            foreach (var item in _currectInfo.Authors)
            {
                var child = new TextBlock() { Text = item.Name, Margin = new(0, 0, 5, 0) };
                DetailAuthorsStack.Children.Add(child);
            }
            DetailTagsTB.Text = "";
            foreach (var item in _currectInfo.Categories)
            {
                DetailTagsTB.Text += $"#{item.Name} ";
            }
            DetailImg.Source = control.Img.Source;
            indexes = _currectInfo.LatestFilesIndexes.OrderByVersion(true);
            var game = GamesCaller.GetSelected();
            if (game is IModLoaderOption g)
            {
                foreach (var item in indexes)
                {
                    if ((int)item.ModLoader == (int)g.ModLoaderType&&item.GameVersion == g.BaseVersion)
                    {
                        ForYourCurrectHint.IsVisible= true;
                        ForYourCurrect.IsVisible = true;
                        ForYourCurrect.Child = new ForgeFileItem(_currectInfo.Id, item);
                        break;
                    }
                }
            }
            Others.Children.Clear();
            versions.Clear();
            OtherIndexs.SelectedItem = -1;
            foreach (var item in indexes)
            {
                var v = Version.Parse(item.GameVersion);
                if (!versions.Contains(v))
                {
                    versions.Add(v);
                }
                //Others.Children.Add(new ForgeFileItem(info.Id, item));
            }
            versions.OrderByDescending(x=>x);
            if (versions.Count > 0)
            {
                OtherIndexs.Items = versions;
                OtherIndexs.SelectedIndex = 0;
            }
        }
    }
}
