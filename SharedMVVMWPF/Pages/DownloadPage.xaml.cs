using MEFL.PageModelViews;
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
using ModelView = MEFL.PageModelViews.DownloadPageModelView;
using MEFL.Controls;
using System.Collections;
using CoreLaunching.JsonTemplates;

namespace MEFL.Pages
{
    /// <summary>
    /// DownloadPage.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadPage : MyPageBase
    {
        public bool Inied;
        public DownloadPage()
        {
            this.DataContext = ModelView.ModelView;
            ModelView.ModelView.PropertyChanged += ModelView_PropertyChanged;
            ModelView.UI=this;
            InitializeComponent();
        }
        int ChildCount = 0;
        private void ModelView_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var mv = sender as ModelView;
            if(e.PropertyName == "ItemSource")
            {
                Dispatcher.Invoke(() => {
                    for (int i = 0; i < MySP.Children.Count; i++)
                    {
                        GC.SuppressFinalize(MySP.Children[i]);
                        MySP.Children.RemoveAt(i);
                        i--;
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    ChildCount = MySP.Children.Count;
                });
                if (mv.ItemSource.Count >= ChildCount)
                {
                    for (int i = 0; i < mv.ItemSource.Count; i++)
                    {
                        if (ChildCount > i)
                        {
                            Dispatcher.Invoke(() => {
                                (MySP.Children[i] as MyItemsCard).ItemsSource = mv.ItemSource[i];
                                (MySP.Children[i] as MyItemsCard).Title = mv.ItemSource[i].VersionMajor;
                            });
                        }
                        else
                        {
                            Dispatcher.Invoke(() => {
                                MySP.Children.Add(new MyItemsCard() { ItemsSource = mv.ItemSource[i], Title = mv.ItemSource[i].VersionMajor });
                            });
                            ChildCount++;
                        }
                    }
                }
                Dispatcher.Invoke(() => {
                    for (int i = 0; i < MySP.Children.Count; i++)
                    {
                        if (((MySP.Children[i] as MyItemsCard).ItemsSource as IList).Count == 0)
                        {
                            MySP.Children.RemoveAt(i);
                            i--;
                        }
                    }
                    for (int i = 0; i < MySP.Children.Count; i++)
                    {
                        if(i > 2)
                        {
                            (MySP.Children[i] as MyItemsCard).IsSwaped = true;
                        }
                    }
                });
                mv.IsRefreshing = false;
            }
            else if (e.PropertyName == "IsRefreshing")
            {
                Dispatcher.Invoke(() => {
                    if (mv.IsRefreshing == true)
                    {
                        RefreshingBox.Visibility = Visibility.Visible;
                        MySP.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        RefreshingBox.Visibility = Visibility.Hidden;
                        MySP.Visibility = Visibility.Visible;
                    }
                });
            }
        }

        private void MyItemsCardItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
            {
                if (((App.Current.Resources["MainPage"] as Grid).Children[i] as MyPageBase).Tag == "PickUP" ||
                    ((App.Current.Resources["MainPage"] as Grid).Children[i] as MyPageBase).Tag == "RenameFolder"
                )
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new SpecialPages.PickUpAFolder() { Tag = "PickUP", Visibility = Visibility.Hidden });
            MyPageBase From = null;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("PickUP", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }
        }
    }
}
