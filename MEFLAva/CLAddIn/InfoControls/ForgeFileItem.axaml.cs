using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using CoreLaunching.DownloadAPIs.Forge;
using CoreLaunching.Forge;
using MEFL.Contract;
using ModLoaderType = CoreLaunching.DownloadAPIs.Forge.ModLoaderType;

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
                    this.DataContext = info;
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
                    RootBtn.Click += RootBtn_Click;
                });
            }).Start();
        }
        static OpenFileDialog _o = new();
        private async void RootBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(DataContext is ForgeFileInfo f&&Application.Current.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime app)
            {
                _o.Title = "ѡ���ļ�����";
                _o.Filters = new List<FileDialogFilter>() { new FileDialogFilter() {Name="jar ��",Extensions=new() { "*.jar" } } };
                var game = Callers.GamesCaller.GetSelected();
                if (game != null)
                {
                    _o.Directory = Path.Combine(game.GameFolder,"mods");
                }
                else
                {
                    _o.Directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "downloads");
                }
                _o.InitialFileName = f.DisplayName;
                _o.AllowMultiple= false;
                var chosed = await _o.ShowAsync(app.MainWindow);
                if (chosed!=null&&string.IsNullOrEmpty(chosed.First())!=true)
                {
                    Callers.DownloaderCaller.CallSingle(f.DownloadUrl, chosed.First());
                }
            }
        }
    }
}
