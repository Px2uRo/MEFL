using MEFL.Controls;
using System;
using System.Collections.Generic;
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

namespace MEFL.CLAddIn.Pages
{
    /// <summary>
    /// MCERPage.xaml 的交互逻辑
    /// </summary>
    public partial class MCERPage : MyPageBase
    {
        public MCERPage()
        {
            InitializeComponent();
            Checked.IsChecked = true;
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            foreach (FrameworkElement item in (this.Content as Panel).Children)
            {
                item.Visibility = Visibility.Hidden;
            }
            FindControl.FromTag((sender as FrameworkElement).Tag,this.Content as Panel)[0].Visibility=Visibility.Visible;
        }
    }
}
