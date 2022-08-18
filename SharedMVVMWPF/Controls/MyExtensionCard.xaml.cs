using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
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

namespace MEFL.Controls
{
    /// <summary>
    /// MyExtensionCard.xaml 的交互逻辑
    /// </summary>
    public partial class MyExtensionCard : MEFL.Controls.MyCard
    {
        public MyExtensionCard()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            //Hosting.IsOpen==
            if (false)
            {
                MessageBox.Show("请启用插件后重试");
            }
            else 
            {
                try
                {
                    Process.Start("explorer.exe", (sender as Hyperlink).NavigateUri.AbsoluteUri.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }

    public class HostingModelView : PageModelViews.PageModelViewBase
    {
        Hosting Hosting;

        public bool IsOpen
        {
            get { return Hosting.IsOpen; }
            set { 

                Hosting.IsOpen = value;
                Invoke(nameof(IsOpen)); 
            }
        }

        public HostingModelView(Hosting hosting)
        {
            Hosting = hosting;
        }
    }
}
