using Avalonia;
using Avalonia.Controls;
using MEFL.Arguments;
using MEFL.Contract;

namespace ServerInstaller
{
    public partial class InstallServerPage : UserControl,IInstallContextMenuPage
    {
        public InstallServerPage()
        {
            InitializeComponent();
            CancelBtn.Click += CancelBtn_Click;
            InstallBtn.Click += InstallBtn_Click;
            NameTP.PropertyChanged += NameTP_PropertyChanged;
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
            var vp = System.IO.Path.Combine(_dotMCPath, "versions", NameTP.Text);
            if (Directory.Exists(vp))
            {
                DesTB.Text = ("想卡BUG吗？但是文件夹已存在");
                InstallBtn.IsEnabled = false;
                return;
            }
            else
            {
                Directory.CreateDirectory(vp);
            }
            var args = new InstServerBaseArgs((bool)OnlineOption.IsChecked,(bool)WhitelistOption.IsChecked,PortTB.Text,ServerID.Text);
            _p = new(args, _info,_javas,_dotMCPath);
            Solved.Invoke(this,_p);
        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this,EventArgs.Empty);
        }

        LauncherWebVersionInfo _info;
        FileInfo[] _javas;
        string _dotMCPath;
        public InstallServerPage(LauncherWebVersionInfo info, FileInfo[] javas, string dotMCPath)
        {
            VersionTB.Text=info.Id.ToString();
            _javas = javas;
            _dotMCPath = dotMCPath;
            _info= info;
        }
        InstServer _p;

        public event EventHandler<InstallProcess> Solved;
        public event EventHandler<EventArgs> Quited;

        public void WindowSizeChanged(Size size)
        {

        }
    }
}
