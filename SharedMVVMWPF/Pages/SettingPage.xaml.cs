using MEFL.Controls;
using MEFL.PageModelViews;
using Microsoft.Win32;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Timers;
using System.Diagnostics;
using System.ComponentModel;

namespace MEFL.Pages
{
    /// <summary>
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPage : Controls.MyPageBase
    {
        public SettingPage()
        {
            InitializeComponent();
            this.DefalutChangeButton.IsChecked = true;
            timer.Elapsed += T_Elapsed;
        }
        private void propChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void ChangePageContentButton_Checked(object sender, RoutedEventArgs e)
        {
            var btn = sender as ChangePageContentButton;
            btn.Show(btn.Tag.ToString(), this.Content as Panel);
        }
        Thread t;
        System.Timers.Timer timer = new System.Timers.Timer(1000);
        private void SearchJava(object sender, RoutedEventArgs e)
        {
            (this.Resources["SPMV"] as SettingPageModelView).EnableSearchJava = false;
            t = new Thread(() =>
            {
                (this.Resources["SPMV"] as SettingPageModelView).Javas = APIData.APIModel.SearchJavas();
                timer.Start();
            });
            t.Start();
        }
        private void ReSetJVMArgs(object sender, RoutedEventArgs e)
        {
            (this.Resources["SPMV"] as SettingPageModelView).OtherJVMArgs = "-XX:+UseG1GC -XX:-UseAdaptiveSizePolicy -XX:-OmitStackTraceInFastThrow -Dfml.ignoreInvalidMinecraftCertificates=True -Dfml.ignorePatchDiscrepancies=True -Dlog4j2.formatMsgNoLookups=true";
        }
        private void OpenWebSite(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe",(sender as Button).Tag.ToString());
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (APIData.APIModel.SearchJavaThreadIsOK == true)
            {
                if ((this.Resources["SPMV"] as SettingPageModelView).Javas.Count > 0)
                {
                    (this.Resources["SPMV"] as SettingPageModelView).SelectedJavaIndex = 0;
                    (this.Resources["SPMV"] as SettingPageModelView).Invoke("SelectedJavaIndex");
                }
                (this.Resources["SPMV"] as SettingPageModelView).Invoke("Javas");
                (this.Resources["SPMV"] as SettingPageModelView).EnableSearchJava = true;
                (sender as System.Timers.Timer).Stop();
            }
        }

        private void AddNewJava(object sender, RoutedEventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.ShowDialog();
            o = null;
        }
    }
}
