using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using CoreLaunching;
using CoreLaunching.DownloadAPIs.Forge;
using CoreLaunching.Forge;
using DynamicData;
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
        string _chineseName;
        public ForgeFileItem(ForgeFileInfo info):this()
        {
            NameTB.Text = info.DisplayName;
            if (!(info.ModLoader==ModLoaderType.Any))
            {
                LoaderTB.Text = info.ModLoader.ToString();
            }
            DownloadTB.Text = info.ChineseDownloadCount;
        }
        IList<ForgeFileInfo> Dpendencies;
        public ForgeFileItem(int modId,LatestFilesIndex index, string chineseName) :this()
        {
            _chineseName = chineseName;
            new Thread(() =>
            {
                var info = ForgeResourceFinder.GetFromModIdAndFileId(modId, index.FileId);
                info.ChineseName = _chineseName;
                if (info.Dependencies.Length > 0)
                {
                    Dpendencies = new List<ForgeFileInfo>();
                }
                foreach (var item in info.Dependencies)
                {
                    if(item.RelationType == RelationType.RequiredDependency)
                    {
                        var files = ForgeResourceFinder.GetFilesFromModId(item.ModId);
                        Version thsV = new Version();
                        string thisLoader=info.ModLoader.ToString().ToLower();
                        foreach (var v in info.GameVersions)
                        {
                            if(Version.TryParse(v,out var ve))
                            {
                                thsV = ve;
                            }
                        }
                        foreach (var file in files.Data)
                        {
                            bool canbreak = false;
                            foreach (var vf in file.GameVersions)
                            {
                                if (Version.TryParse(vf, out var ve2))
                                {
                                    if(thsV == ve2)
                                    {
                                        if (thsV>=new Version(1,13,0))
                                        {
                                            if (file.GameVersions.Where(x=>x.ToLower()==thisLoader).Count()>0)
                                            {
                                                Dpendencies.Add(file);
                                                canbreak = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Dpendencies.Add(file);
                                            canbreak = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if(canbreak)
                            {
                                break;
                            }
                        }
                    }
                }
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
        static SaveFileDialog _s;
        private async void RootBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(DataContext is ForgeFileInfo f&&Application.Current.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime app)
            {
                _s = new();
                _s.Title = "选择文件名称";
                _s.Filters = new List<FileDialogFilter>() { new FileDialogFilter() {Name="jar 包",Extensions=new() { "*.jar" } } };
                var game = Callers.GamesCaller.GetSelected();
                if (game != null)
                {
                    _s.Directory = Path.Combine(game.GameFolder,"mods");
                }
                else
                {
                    _s.Directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "downloads");
                }
                _s.InitialFileName = f.GetLocalName();
                // _s.AllowMultiple= false;
                var chosed = await _s.ShowAsync(app.MainWindow);
                if (!string.IsNullOrEmpty(chosed))
                {
                    var red = UrlUtil.Redirect(f.DownloadUrl);
                    Callers.DownloaderCaller.CallSingle(red, chosed);
                    var fo = Path.GetDirectoryName(chosed);
                    foreach (var item in Dpendencies)
                    {
                        Callers.DownloaderCaller.CallSingle(item.DownloadUrl,Path.Combine(fo,item.Name));
                    }
                }
                GC.SuppressFinalize(_s);
            }
        }
    }
}
