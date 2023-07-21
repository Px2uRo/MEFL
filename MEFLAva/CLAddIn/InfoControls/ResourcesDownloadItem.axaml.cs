using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CoreLaunching.DownloadAPIs.Forge;
using CoreLaunching.DownloadAPIs.Interfaces;
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

        public ResourcesDownloadItem(IModInfo info):this()
        {
            DataContext= info;
            var c = ModCache.Caches.Where(x => x.slugs.Contains(info.Slug));
            if (c.Count()>0)
            {
                if (!string.IsNullOrEmpty(c.First().majorName))
                {
                    NameTB.Text = c.First().majorName;
                }
                else
                {
                    NameTB.Text = info.Name;
                }
            }
            else
            {
                NameTB.Text = info.Name;
            }
            DesTB.Text = info.Description;
            var versions = new List<Version>();
            foreach (var item in info.GetDownloadLinks())
            {
                versions.Add(Version.Parse(item.Versions.FirstOrDefault()));
            }
            versions.Sort();
            foreach (var item in info.Tags)
            {
                TagsTB.Text += $"#{item} ";
            }
            SupportVersionTB.Text = $"{versions.First()}-{versions.Last()}";
            DownloadCount.Text = info.DownloadCountsInfo;
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
                            var ms = new MemoryStream(clt.DownloadData(info.GetLogoUrl()));
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
