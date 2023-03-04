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

namespace MEFL.CLAddIn
{
    /// <summary>
    /// SolveDupNamePage.xaml 的交互逻辑
    /// </summary>
    public partial class SolveDupNamePage : UserControl
    {
        string fp;
        string _native;
        MEFLDownloader _downloader;
        DownloadSource[] _sources;
        public SolveDupNamePage(string native,string FolderPath,string Id,MEFLDownloader downloader, DownloadSource[] sources)
        {
            fp = FolderPath;
            _native = native;
            _sources= sources;
            _downloader = downloader;
            InitializeComponent();
            NameBox.TextChanged += NameBox_TextChanged;
            NameBox.Text = Id;
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
                DownloadBtn.IsEnabled = true;
                InfoBox.Text = String.Empty;
            }
        }
            
        private void Button_Cliked(object sender,RoutedEventArgs e)
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
            (this.DataContext as Contract.InstallProgressInput).Progress = _downloader.InstallMinecraft(_native,fp,_sources,new(NameBox.Text,"",""));
            _downloader = null;
            (this.DataContext as Contract.InstallProgressInput).NowDownload();
        }
    }
}
