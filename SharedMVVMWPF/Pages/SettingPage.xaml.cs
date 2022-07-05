using MEFL.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void ChangePageContentButton_Checked(object sender, RoutedEventArgs e)
        {
            var btn = sender as ChangePageContentButton;
            btn.Show(btn.Tag.ToString(), this.Content as Panel);
        }
        private void MyComboBox_DropDownOpened(object sender, EventArgs e)
        {

        }
    }
}
