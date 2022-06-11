using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Hosting == null)
            {
                throw new Exception("未设置 Hosting 属性");
            }
            if(Hosting.ExceptionInfo != null)
            {
                base.Title = $"加载文件 {Hosting.FileName} 错误";
                PART_THE_Content.Children.Clear();
                PART_THE_Content.Children.Add
                    (new TextBlock() { Text = Hosting.ExceptionInfo.Message });
            }
            else
            {
                if (Hosting.BaseInfo.Title != null)
                {
                    this.Title = Hosting.BaseInfo.Title;
                }
                else
                {
                    this.Title = Hosting.FileName;
                }
            }
            PART_THE_Content.DataContext = Hosting;
        }



        public Hosting Hosting
        {
            get { return (Hosting)GetValue(HostingProperty); }
            set { SetValue(HostingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hosting.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HostingProperty =
            DependencyProperty.Register("Hosting", typeof(Hosting), typeof(MyExtensionCard), new PropertyMetadata(null));

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
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
