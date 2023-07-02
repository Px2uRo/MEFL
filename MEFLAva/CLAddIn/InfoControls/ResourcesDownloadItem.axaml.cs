using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CoreLaunching.DownloadAPIs.Forge;
using CoreLaunching.Forge;
using System.Diagnostics;
using System.Net;

namespace MEFL.CLAddIn
{
    public partial class ResourcesDownloadItem : UserControl
    {
        protected override Size ArrangeOverride(Size finalSize)
        {
            RootBtn.Width = finalSize.Width - 2;
            RootBtn.Height = finalSize.Height - 2;
            return base.ArrangeOverride(finalSize);
        }
        public ResourcesDownloadItem()
        {
            InitializeComponent();
        }

        public ResourcesDownloadItem(ForgeModInfo info):this()
        {
            DataContext= info;
            NameTB.Text = info.Name;
            DesTB.Text = info.Summary;
            var versions = new List<Version>();
            foreach (var item in info.LatestFilesIndexes)
            {
                versions.Add(Version.Parse(item.GameVersion));
            }
            versions.Sort();
            foreach (var item in info.Categories)
            {
                TagsTB.Text += $"#{item.Name} ";
            }
            SupportVersionTB.Text = $"{versions.First()}-{versions.Last()}";
            DownloadCount.Text = info.DownloadCounts;
            try
            {
                foreach (var item in info.Authors)
                {
                    var child = new TextBlock() {Text=item.Name,Margin=new(0, 0, 5, 0)};
                    AuthorsStack.Children.Add(child);
                }
            }
            catch (Exception ex)
            {

            }
                new Thread(() =>
                {
                    try
                    {
                        using (var clt = new WebClient())
                        {
                            var ms = new MemoryStream(clt.DownloadData(info.Logo.Url));
                            Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                Img.Source = new Bitmap(ms);
                                ms.Close();
                                ms.Dispose();
                            });
                        }
                    }
                    catch (Exception)
                    {

                    }
                }).Start();
        }
    }
}
