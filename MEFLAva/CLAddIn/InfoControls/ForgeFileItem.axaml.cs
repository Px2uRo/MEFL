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
                    if (!(info.ModLoader == ModLoaderType.Any))
                    {
                        LoaderTB.Text = info.ModLoader.ToString();
                    }
                    DownloadTB.Text = info.ChineseDownloadCount;
                });
            }).Start();
        }
    }
}
