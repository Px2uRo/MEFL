using MEFL.APIData;
using MEFL.Contract;
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
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            ((sender as FrameworkElement).DataContext as GameInfoBase).Delete();
            (this.DataContext as RealMainPageModelView).GameInfoConfigs.Remove((sender as FrameworkElement).DataContext as GameInfoBase);
            (this.DataContext as RealMainPageModelView).Invoke("GameInfoConfigs");
        }
        private void SetItemToFavorite(object sender, RoutedEventArgs e)
        {
            (App.Current.Resources["RMPMV"] as RealMainPageModelView).MyFolders[APIModel.SelectedFolderIndex].SetToFavorite((sender as FrameworkElement).DataContext as GameInfoBase);
            (App.Current.Resources["RMPMV"] as RealMainPageModelView).Invoke("GameInfoConfigs");
        }
        private void ItemSetting(object sender, RoutedEventArgs e)
        {
            foreach (MyPageBase item in FindControl.FromTag("SettingGamePage", (App.Current.Resources["MainPage"] as Grid)))
            {
                (App.Current.Resources["MainPage"] as Grid).Children.Remove(item);
            }
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new SpecialPages.GameSettingPage() { Tag = "SettingGamePage", Visibility = Visibility.Hidden, Content = ((sender as FrameworkElement).DataContext as GameInfoBase).SettingsPage, DataContext = new GenerlSettingGameModelView((sender as FrameworkElement).DataContext as GameInfoBase) });
            MyPageBase From = new MyPageBase();
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("SettingGamePage", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }
        }

        private void RealMainPageModelView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void MyComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as RealMainPageModelView).CurretGame=
                ((sender as Controls.MyItemsCardItem).DataContext 
                as Contract.GameInfoBase);
        }
    }
}
