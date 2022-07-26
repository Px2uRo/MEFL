using MEFL.Controls;
using MEFL.PageModelViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MEFL.Pages
{
    /// <summary>
    /// RealMainPage.xaml 的交互逻辑
    /// </summary>
    public partial class RealMainPage : MEFL.Controls.MyPageBase
    {

        public RealMainPage()
        {
            InitializeComponent();
            (this.DataContext as RealMainPageModelView).PropertyChanged += RealMainPageModelView_PropertyChanged;
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeGameBorder.Visibility = Visibility.Visible;
        }

        private void RealMainPageModelView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void MyComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as RealMainPageModelView).CurretGame=
                ((sender as Controls.MyItemsCardItem).DataContext 
                as Contract.GameInfoBase);
            RegManager.Write("CurretGame", (this.DataContext as RealMainPageModelView).CurretGame.RootFolder);
        }
    }
}
