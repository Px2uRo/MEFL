using Avalonia.Controls;
using CLAddIn.Properties;
using CoreLaunching.JsonTemplates;
using CoreLaunching;
using MEFL.Arguments;
using MEFL.Contract;
using CoreLaunching.Forge;
using Avalonia;
using CoreLaunching.DownloadAPIs.Forge;

namespace CLAddIn.Views
{
    public partial class InstallPage : UserControl,IInstallPage
    {

        public LauncherWebVersionInfo Info { get; set; }

        string _dotMCPath;
        WebForgeInfo _wInfo;
        bool _insfor = false;

        public InstallPage()
        {
            InitializeComponent();
            CancelBtn.Click += CancelBtn_Click;
            InstallBtn.Click += InstallBtn_Click;
            NameTP.PropertyChanged += NameTP_PropertyChanged;
            InstallForgeCB.Checked += InstallForgeCB_Checked;
            InstallForgeCB.Unchecked += InstallForgeCB_Unchecked;
        }

        private void InstallForgeCB_Unchecked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ForgeContentsSC.IsVisible = false;
            _insfor= false;
        }

        private void InstallForgeCB_Checked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ForgeContentsSC.IsVisible= true;
            if (!BMCLForgeDownloadAPI.GetSupporttedVersions().Contains(Info.Id))
            {
                InstallForgeCB.IsChecked = false;
                InstallForgeCB.IsVisible = false;
                InstallForgeCB.Content = "对不起，但是这个版本不支持Forge";
                return;
            }
            else
            {
                if (ForgeContentsSP.Children.Count==0)
                {
                    var items = BMCLForgeDownloadAPI.GetKnownUrlsFromMcVersion(Info.Id);
                    for (int i = 0; i < items.Length; i++)
                    {
                        var item = items[i]; 
                        var radio = new RadioButton();
                        radio.DataContext = item;
                        radio.Content = item.Version;
                        radio.Checked += Radio_Checked;
                        if (i == 0)
                        {
                            radio.IsChecked = true;
                        }
                        ForgeContentsSP.Children.Add(radio);
                    }
                }
            }
            _insfor= true;
        }

        private void Radio_Checked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var ninfo = (WebForgeInfo)((sender as RadioButton).DataContext);
            if (_wInfo != null)
            {
                if (NameTP.Text.Contains(_wInfo.Version.ToString()))
                {
                    NameTP.Text = NameTP.Text.Replace(_wInfo.Version.ToString(), ninfo.Version.ToString());
                }
                else
                {
                    NameTP.Text += $"_forge_{ninfo.Version}";
                }
            }
            else
            {
                NameTP.Text += $"_forge_{ninfo.Version}";
            }
            _wInfo = ninfo;
        }

        private void NameTP_PropertyChanged(object? sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBlock.TextProperty)
            {
                var vp = System.IO.Path.Combine(_dotMCPath, "versions", NameTP.Text);
                if (vp.EndsWith("."))
                {
                    InstallBtn.IsEnabled = false;
                    DesTB.Text = $"文件夹不以小数点结尾";
                }
                else if (Directory.Exists(vp))
                {
                    InstallBtn.IsEnabled = false;
                    DesTB.Text = $"文件夹 {NameTP.Text} 已存在";
                }
                else
                {
                    InstallBtn.IsEnabled = true;
                    DesTB.Text = $"即将安装{NameTP.Text}";
                }
            }
        }

        private void InstallBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var args = new List<InstallArguments>();
            var vp = System.IO.Path.Combine(_dotMCPath, "versions", NameTP.Text);
            if (Directory.Exists(vp))
            {
                DesTB.Text=("想卡BUG吗？但是文件夹已存在");
                InstallBtn.IsEnabled = false;
                return;
            }
            else
            {
                Directory.CreateDirectory(vp);
            }
            args.Add(new(_javas,NameTP.Text, null, null,Info));
            if (_insfor)
            {
                args.Add(new InstallArgsWithForge(_javas,args.First(),_wInfo));
            }
            Solved?.Invoke(this, args);
        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this, e);
        }

        public void WindowSizeChanged(Size size)
        {

        }

        FileInfo[] _javas;
        public InstallPage(FileInfo[] javas, LauncherWebVersionInfo info,string dotMCPath) : this()
        {
            _javas = javas;
            Info = info;
            _dotMCPath = dotMCPath;
            DesTB.Text= $"即将安装{info.Id}";
            NameTP.Text = info.Id;
        }

        public event EventHandler<IEnumerable<InstallArguments>> Solved;
        public event EventHandler<EventArgs> Quited;
    }
}
