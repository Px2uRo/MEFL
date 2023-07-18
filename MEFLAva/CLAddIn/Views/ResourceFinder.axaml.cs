using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using CoreLaunching.DownloadAPIs.Forge;
using CoreLaunching.DownloadAPIs.Interfaces;
using CoreLaunching.Forge;
using CoreLaunching.MicrosoftAuth;
using MEFL.Callers;
using MEFL.CLAddIn;
using MEFL.CLAddIn.Export;
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
            FitherVersionCB.Width= finalSize.Width - 165;
            return b;
        }
        public ResourceFinder()
        {
            InitializeComponent();
            CancelDetailBtn.Click += CancelDetailBtn_Click;
            OtherIndexs.SelectionChanged += OtherIndexs_SelectionChanged;
            SearchBtn.Click += SearchBtn_Click;
            LoadVersion();
            LoadHot();
        }

        private void SearchBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SearchBtn.IsEnabled = false;
            new Thread(() =>
            {
                var can = ForgeResourceFinder.FindMinecraft(out var res, out var er, keyWords: FitherTB.Text);
                if (can)
                {
                    LoadResult(res.Data, FitherTB.Text);
                }
                else
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        HotInfoTB.Text = er;
                    });
                }
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    SearchBtn.IsEnabled = true;
                });
            }).Start();
            
        }

        private void LoadVersion()
        {
            var items = new List<string>();
            var v = CoreLaunching.DownloadAPIs.Mojang.GetVersion().Where(x=>x.Type== "release");
            foreach (var item in v)
            {
                items.Add(item.Id);
            }
            FitherVersionCB.Items = items;
            if (FitherVersionCB.SelectedIndex == -1&&items.Count!=0) 
            {
                var game = GamesCaller.GetSelected();
                if (game is IModLoaderOption g)
                {
                    if (items.Contains(g.BaseVersion))
                    {
                        FitherVersionCB.SelectedIndex = items.IndexOf(g.BaseVersion);
                    }
                    else
                    {
                        FitherVersionCB.SelectedIndex = 0;
                    }
                }
                else
                {
                    FitherVersionCB.SelectedIndex = 0;
                }
            }
        }

        IModInfo _currectInfo;
        IEnumerable<IModFileInfo> indexes;
        private void OtherIndexs_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var s = sender as ComboBox;
            if(s.SelectedIndex != -1)
            {
                if (_currectInfo is ForgeModInfo i)
                {

                    var item = s.SelectedItem;
                    Others.Children.Clear();
                    var others = indexes.Where(x => x.Versions.First() == item.ToString());
                    foreach (var ite in others)
                    {
                        var child = new ForgeFileItem(i.Id, ite as LatestFilesIndex);
                        Others.Children.Add(child);
                    }
                }
            }
        }

        private void CancelDetailBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ListsPage.IsVisible = true;
            DetailStack.IsVisible = false;
        }

        private void LoadResult(IEnumerable<IModInfo> mods,string title)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                HotInfoBD.IsVisible = false;
                TabItem res = new TabItem();
                var btn = new ResultHeader(title) { DataContext = res};
                var stack = new StackPanel();
                foreach (var mod in mods)
                {
                    var child = new ResourcesDownloadItem(mod);
                    child.RootBtn.Click += RootBtn_Click;
                    stack.Children.Add(child);
                }
                res.Header = btn;
                res.Content = stack;
                (MyContent.Items as IList<object>).Add(res);
                MyContent.SelectedIndex = MyContent.ItemCount-1;
            });    
        }

        private void LoadHot()
        {
            new Thread(() =>
            {

                var can = ForgeResourceFinder.GetFeatured(out var res,out var e);
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
            _currectInfo = control.DataContext as IModInfo;
            DetailNameTB.Text = _currectInfo.Name;
            DetailDesTB.Text = _currectInfo.Description;
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
            foreach (var item in _currectInfo.Tags)
            {
                DetailTagsTB.Text += $"#{item} ";
            }
            DetailImg.Source = control.Img.Source;
            indexes = _currectInfo.GetDownloadLinks().OrderByVersion(true);
            var game = GamesCaller.GetSelected();
            if (game is IModLoaderOption g)
            {
                foreach (var item in indexes)
                {
                    if ((int)item.ModLoader == (int)g.ModLoaderType&&item.Versions.First() == g.BaseVersion)
                    {
                        ForYourCurrectHint.Height = double.NaN;
                        ForYourCurrectHint.IsVisible= true;
                        ForYourCurrect.IsVisible = true;
                        if(_currectInfo is ForgeModInfo c)
                        {
                            ForYourCurrect.Child = new ForgeFileItem(c.Id,item as LatestFilesIndex);
                        }
                        break;
                    }
                }
            }
            else
            {
                ForYourCurrectHint.Height= 0;
            }
            Others.Children.Clear();
            versions.Clear();
            OtherIndexs.SelectedItem = -1;
            foreach (var item in indexes)
            {
                var v = Version.Parse(item.Versions.First());
                if (!versions.Contains(v))
                {
                    versions.Add(v);
                }
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
