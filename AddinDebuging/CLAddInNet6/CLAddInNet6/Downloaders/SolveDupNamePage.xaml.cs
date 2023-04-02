using CoreLaunching.Forge;
using MEFL.Arguments;
using MEFL.CLAddIn.Downloaders;
using MEFL.CLAddIn.GameTypes;
using MEFL.Contract;
using MEFL.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace MEFL.CLAddIn
{
    internal class InstallArgsWithForge : InstallArguments
    {
        public InstallArgsWithForge(string versionName, string customGameFolder, string gameIcon) : base(versionName, customGameFolder, gameIcon)
        {

        }
        private WebForgeInfo _forge;

        public WebForgeInfo Forge
        {
            get { return _forge; }
            set { _forge = value; }
        }

    }
    /// <summary>
    /// InstallPage.xaml 的交互逻辑
    /// </summary>
    public partial class InstallPage : UserControl
    {
        string fp;
        string _native;
        string oid;
        bool wannaInstallForge = false;
        WebForgeInfo selectedforge = null;
        InstallArguments _args = new("", "", "");
        InstallArguments args { get => _args; set {
                GC.SuppressFinalize(_args); _args = value;
            } }
        MEFLDownloader _downloader;
        DownloadSource[] _sources;
        string[] _usingLocalFiles;
        static BitmapImage pubIcon = new BitmapImage(new Uri("pack://application:,,,/RealseTypeLogo.png", UriKind.Absolute));
        public InstallPage(string native, string FolderPath, string Id, MEFLDownloader downloader, DownloadSource[] sources, string[] usingLocalFiles)
        {
            fp = FolderPath;
            _native = native;
            _sources = sources;
            _downloader = downloader;
            _usingLocalFiles = usingLocalFiles;
            InitializeComponent();
            NameBox.TextChanged += NameBox_TextChanged;
            oid = Id;
            NameBox.Text = Id;
            ImageBox.Source = pubIcon;
        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vp = System.IO.Path.Combine(fp, "versions", NameBox.Text);
            if (vp.EndsWith("."))
            {
                DownloadBtn.IsEnabled = false;
                InfoBox.Text = $"文件夹不以小数点结尾";
            }
            else if (Directory.Exists(vp))
            {
                DownloadBtn.IsEnabled = false;
                InfoBox.Text = $"文件夹 {NameBox.Text} 已存在";
            }
            else
            {
                if (!wannaInstallForge)
                {
                    DownloadBtn.IsEnabled = true;
                    InfoBox.Text = $"即将安装 {NameBox.Text}";
                }
                else
                {
                    if (selectedforge==null)
                    {
                        DownloadBtn.IsEnabled = false;
                        InfoBox.Text = $"对不起，但是你还没选择要安装的 Forge";
                    }
                    else
                    {
                        DownloadBtn.IsEnabled = true;
                        InfoBox.Text = $"即将安装 {NameBox.Text}";
                    }
                }
            }
        }

        private void Button_Cliked(object sender, RoutedEventArgs e)
        {
            var vp = System.IO.Path.Combine(fp, "versions", NameBox.Text);
            if (Directory.Exists(vp))
            {
                MyMessageBox.Show("想卡BUG吗？但是文件夹已存在");
            }
            else
            {
                Directory.CreateDirectory(vp);
            }
            if(wannaInstallForge&& selectedforge!=null)
            {
                var narg = new InstallArgsWithForge(NameBox.Text, "", "");
                narg.Forge = selectedforge;
                args = narg;
            }
            else
            {
                args = new(NameBox.Text, "", "");
            }
            (this.DataContext as Contract.InstallProgressInput).Progress =
                _downloader.InstallMinecraft
                (_native, fp, _sources,
                args, _usingLocalFiles);
            _downloader = null;
            (this.DataContext as Contract.InstallProgressInput).NowDownload();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            E_Button.Visibility = Visibility.Visible;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            E_Button.Visibility = Visibility.Hidden;
        }

        private void ChoosePictrue(object sender, RoutedEventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {

        }


        private void ForgeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!BMCLForgeHelper.KnownSupportedVersions.Contains(this.oid))
            {
                ForgeCheckBox.Content = $"对不起，但是 Forge 不支持 {oid} 版本";
                ForgeCheckBox.IsChecked = false;
                wannaInstallForge = false;
            }
            else
            {
                ImageBox.Source = new PngBitmapDecoder(CLGameType.ForgeStream, BitmapCreateOptions.None, BitmapCacheOption.Default).Frames[0];
                ForgeListCard.Visibility = Visibility.Visible;
                wannaInstallForge = true;
                if(ForgeListCard.Content == null)
                {
                    ForgeListCard.Content=new StackPanel();
                    foreach (var item in BMCLForgeHelper.GetKnownUrlsFromMcVersion(oid))
                    {
                        var newEl = new WebForgeInfoUI(item);
                        newEl.MouseDown += NewEl_MouseDown;
                        (ForgeListCard.Content as StackPanel).Children.Add(newEl);
                    }
                }
                NameBox_TextChanged(null, null);
            }

        }

        private void NewEl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedforge = (sender as WebForgeInfoUI).Info;
            NameBox.Text = $"{oid}-forge-{selectedforge.Version}";
        }

        private void ForgeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            NameBox.Text = $"{oid}";
            ImageBox.Source = pubIcon;
            ForgeListCard.Visibility = Visibility.Hidden;
            wannaInstallForge = false;
            NameBox_TextChanged(null, null);
        }

    }
}
