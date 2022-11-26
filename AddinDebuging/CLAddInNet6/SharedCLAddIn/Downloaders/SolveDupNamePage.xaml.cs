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
        public SolveDupNamePage(string native,string FolderPath,string Id)
        {
            fp = FolderPath;
            _native = native;
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
            (this.DataContext as Contract.LauncherProgressResult).Progress.NativeLocalPairs.Clear();
            (this.DataContext as Contract.LauncherProgressResult).Progress.NativeLocalPairs.Add(_native,System.IO.Path.Combine(fp, "versions", NameBox.Text,$"{NameBox.Text}.json"));
            (this.DataContext as Contract.LauncherProgressResult).NowDownload();
        }
    }
}
