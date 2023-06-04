using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using CoreLaunching.Forge;
using CoreLaunching.MicrosoftAuth;
using MEFL.Callers;
using MEFL.CLAddIn;
using MEFL.Contract;
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
            LoadHot();
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

        private void LoadDetail(ResourcesDownloadItem control)
        {
            var info = control.DataContext as ForgeModInfo;
            DetailNameTB.Text = info.Name;
            DetailDesTB.Text = info.Summary;
            var versions = new List<Version>();
            DetailSupportVersionTB.Text = control.SupportVersionTB.Text;
            DetailDownloadCount.Text = info.ChineseDownloadCount;
            DetailAuthorsStack.Children.Clear();
            foreach (var item in info.Authors)
            {
                var child = new TextBlock() { Text = item.Name, Margin = new(0, 0, 5, 0) };
                DetailAuthorsStack.Children.Add(child);
            }
            DetailTagsTB.Text = "";
            foreach (var item in info.Categories)
            {
                DetailTagsTB.Text += $"#{item.Name} ";
            }
            DetailImg.Source = control.Img.Source;
            var indexes = info.LatestFilesIndexes.OrderByVersion(true);
            var game = GamesCaller.GetSelected();
            if (game is IModLoaderOption g)
            {
                foreach (var item in indexes)
                {
                    if ((int)item.ModLoader == (int)g.ModLoaderType&&item.GameVersion == g.BaseVersion)
                    {
                        ForYourCurrectHint.IsVisible= true;
                        ForYourCurrect.IsVisible = true;
                        ForYourCurrect.Child = new ForgeFileItem(info.Id, item);
                        break;
                    }
                }
            }
            Others.Children.Clear();
            foreach (var item in indexes)
            {
                Others.Children.Add(new ForgeFileItem(info.Id, item));
            }
        }
    }
}
