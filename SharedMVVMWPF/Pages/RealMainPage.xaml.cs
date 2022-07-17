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
        #region 一堆字段而已
        Thickness VarMargin = new Thickness(0, 0, 0, 2);
        Thickness VarBorderThickness = new Thickness(5);
        Brush VarBorderBrush = App.Current.Resources["SYTLE_Standard_BorderBrush"] as SolidColorBrush;
        CornerRadius VarConrnerRadius = new CornerRadius(5);
        #endregion

        public RealMainPage()
        {
            InitializeComponent();
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeGameBorder.Visibility = Visibility.Visible;
        }

        private void RealMainPageModelView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GameInfoConfigs")
            {
                MyGamesSP.Children.Clear();

                #region 收藏夹而已
                List<Controls.MyItemsCard> cards = new List<Controls.MyItemsCard>();
                Controls.MyItemsCard favorcard = new Controls.MyItemsCard() {IsAbleToSwap=true, Title = "Favorite", Margin = VarMargin, BorderThickness = VarBorderThickness, BorderBrush = VarBorderBrush, CornerRadius = VarConrnerRadius, ItemContainerStyle = this.Resources["ContainerStyle"] as Style };
                ObservableCollection<Contract.GameInfoBase> favoritem = new ObservableCollection<Contract.GameInfoBase>();
                foreach (var item in ((RealMainPageModelView)sender).GameInfoConfigs)
                {
                    if (item.IsFavorate)
                    {
                        favoritem.Add(item);
                    }
                }
                if (favoritem.Count > 0)
                {
                    favorcard.ItemsSource = favoritem;
                    cards.Add(favorcard);
                }
                #endregion

                #region 确定有多少卡片而已
                List<string> recorded = new List<string>();
                foreach (var item in ((RealMainPageModelView)sender).GameInfoConfigs)
                {
                    if (recorded.Contains(item.ToString()) != true)
                    {
                        cards.Add(new Controls.MyItemsCard() { IsAbleToSwap = true, Title = item.ToString(), Margin = VarMargin, BorderThickness = VarBorderThickness, BorderBrush = VarBorderBrush, CornerRadius = VarConrnerRadius, ItemContainerStyle = this.Resources["ContainerStyle"] as Style });
                        recorded.Add(item.ToString());
                    }
                }
                #endregion

                #region 给卡片加 ItemSources 而已
                for (int i = 0; i < cards.Count; i++)
                {
                    var cardItemSources = new ObservableCollection<Contract.GameInfoBase>();
                    foreach (var item in ((RealMainPageModelView)sender).GameInfoConfigs)
                    {
                        if (item.ToString() == cards[i].Title.ToString())
                        {
                            cardItemSources.Add(item);
                        }
                    }
                    cards[i].ItemsSource = cardItemSources;
                }
                #endregion

                foreach (var item in cards)
                {
                    MyGamesSP.Children.Add(item);
                }
            }
        }

        private void MyComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as RealMainPageModelView).CurretGame=
                ((sender as Controls.MyItemsCardItem).DataContext 
                as Contract.GameInfoBase);
        }
    }
}
