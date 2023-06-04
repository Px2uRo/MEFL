using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using CoreLaunching.Forge;

namespace MEFL.CLAddIn
{
    public partial class ForgeFileItem : UserControl
    {
        protected override Size ArrangeOverride(Size finalSize)
        {
            var b = base.ArrangeOverride(finalSize);
            RootBtn.Width= b.Width;
            return b;
        }
        public ForgeFileItem()
        {
            InitializeComponent();
        }

        public ForgeFileItem(ForgeFileInfo info):this()
        {
            NameTB.Text = info.DisplayName;
            if (!(info.ModLoader==ModLoaderType.Any))
            {
                LoaderTB.Text = info.ModLoader.ToString();
            }
            DownloadTB.Text = info.ChineseDownloadCount;
        }

        public ForgeFileItem(int modId,LatestFilesIndex index):this()
        {
            new Thread(() =>
            {
                var info = ForgeResourceFinder.GetFromModIdAndFileId(modId, index.FileId);
                Dispatcher.UIThread.InvokeAsync(() =>
                {

                    NameTB.Text = info.DisplayName;
                    if (info.ModLoader == ModLoaderType.Any)
                    {
                        if (info.ReleaseType == FileReleaseType.Release)
                        {
                            Img.Source = BitMaps.Release;
                        }
                        else if (info.ReleaseType == FileReleaseType.Alpha)
                        {
                            Img.Source = BitMaps.Alpha;
                        }
                        else if (info.ReleaseType == FileReleaseType.Beta)
                        {
                            Img.Source = BitMaps.Beta;
                        }
                    }
                    else if (info.ModLoader == ModLoaderType.Fabric)
                    {
                        LoaderTB.Text = info.ModLoader.ToString();
                        if (info.ReleaseType == FileReleaseType.Release)
                        {
                            Img.Source = BitMaps.FabricRelease;
                        }
                        else if (info.ReleaseType == FileReleaseType.Alpha)
                        {
                            Img.Source = BitMaps.FabricAlpha;
                        }
                        else if (info.ReleaseType == FileReleaseType.Beta)
                        {
                            Img.Source = BitMaps.FabricBeta;
                        }
                    }
                    else if (info.ModLoader == ModLoaderType.Forge)
                    {
                        LoaderTB.Text = info.ModLoader.ToString();
                        if (info.ReleaseType == FileReleaseType.Release)
                        {
                            Img.Source = BitMaps.ForgeRelease;
                        }
                        else if (info.ReleaseType == FileReleaseType.Alpha)
                        {
                            Img.Source = BitMaps.ForgeAlpha;
                        }
                        else if (info.ReleaseType == FileReleaseType.Beta)
                        {
                            Img.Source = BitMaps.ForgeBeta;
                        }
                    }
                    DownloadTB.Text = info.ChineseDownloadCount;
                });
            }).Start();
        }
    }
}
