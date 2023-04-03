using MEFL.APIData;
using MEFL.Contract;
using MEFL.Controls;
using MEFL.EventsMethod;
using MEFL.PageModelViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using comp = System.IO.Compression;

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
            (App.Current.Resources["RMPMV"] as RealMainPageModelView).ChangeGameVisblity = Visibility.Visible;
            APIModel.IndexToUI.UpdateUI = true;
            APIModel.IndexToUI.DealWithNew();
        }
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            var res =((sender as FrameworkElement).DataContext as GameInfoBase).Delete();
            if(res == DeleteResult.Canceled)
            {
                return;
            }
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
            bool yes =false;
            //GenerlSettingGameModel.ModelView._gameWatcher = (sender as FrameworkElement).DataContext as GameInfoBase;
            if(FindControl.FromTag("SettingGamePage", (App.Current.Resources["MainPage"] as Grid)).Length == 0)
            {
                yes = true;
            }
            if (yes)
            {
                var newPage = new SpecialPages.GameSettingPage()
                {
                    Tag = "SettingGamePage",
                    Visibility = Visibility.Hidden,
                    Content = ((sender as FrameworkElement).DataContext as GameInfoBase).SettingsPage
                };
                (App.Current.Resources["MainPage"] as Grid).Children.Add(newPage);
            }
            MyPageBase From = null;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("SettingGamePage", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Content = ((sender as FrameworkElement).DataContext as GameInfoBase).SettingsPage;
                #region Events
                ((sender as FrameworkElement).DataContext as GameInfoBase).SettingsPage.OnPageBack -= EventToolkit.SettingsPage_OnPageBack;
                ((sender as FrameworkElement).DataContext as GameInfoBase).SettingsPage.OnRemoved -= EventToolkit.SettingsPage_OnRemoved;
                ((sender as FrameworkElement).DataContext as GameInfoBase).SettingsPage.OnSelected -= EventToolkit.SettingsPage_OnSelected;
                ((sender as FrameworkElement).DataContext as GameInfoBase).SettingsPage.OnListUpdate -= EventToolkit.SettingsPage_OnListUpdate;
                ((sender as FrameworkElement).DataContext as GameInfoBase).SettingsPage.OnPageBack += EventToolkit.SettingsPage_OnPageBack;
                ((sender as FrameworkElement).DataContext as GameInfoBase).SettingsPage.OnRemoved += EventToolkit.SettingsPage_OnRemoved;
                ((sender as FrameworkElement).DataContext as GameInfoBase).SettingsPage.OnSelected += EventToolkit.SettingsPage_OnSelected;
                ((sender as FrameworkElement).DataContext as GameInfoBase).SettingsPage.OnListUpdate += EventToolkit.SettingsPage_OnListUpdate;
                #endregion
                item.Show(From);
            }
        }
        private void RealMainPageModelView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ProcessModelView")
            {
                (sender as RealMainPageModelView).ProcessModelView.PropertyChanged += ProcessModelViewAkaStatuStackPanel_PropertyChanged;
            }
        }
        static DoubleAnimation _dnani = new DoubleAnimation() { EasingFunction=new PowerEase(),Duration=new Duration(TimeSpan.FromSeconds(0.2))};
        private void ProcessModelViewAkaStatuStackPanel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName== "IsStarting")
            {
                if((sender as ProcessModelView).IsStarting == true)
                {
                    Dispatcher.Invoke(() =>
                    {
                        LaunchGameBox.Visibility = Visibility.Hidden;
                        CancelButton.Visibility = Visibility.Visible;
                        _dnani.From = 0;
                        _dnani.To = 80;
                        StatuStackPanel.BeginAnimation(HeightProperty, _dnani);
                    });
                }
            }
            else if(e.PropertyName== "Failed")
            {
                Dispatcher.Invoke(() =>
                {
                    CancelButton.Visibility = Visibility.Hidden;
                LaunchGameBox.Visibility = Visibility.Visible;
                _dnani.From = 80;
                _dnani.To = 0;
                StatuStackPanel.BeginAnimation(HeightProperty, _dnani);
                var MyMBx = MyMessageBox.Show((sender as ProcessModelView).ErrorInfo, e.PropertyName);
                });
            }
            else if (e.PropertyName == "Succeed")
            {
                if ((sender as ProcessModelView).Succeed==true)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ManageProcessesPageModel.ModelView.RunningGames.Add((DataContext as RealMainPageModelView).ProcessModelView.GetProcess);
                        ManageProcessesPageModel.ModelView.RunningGames[ManageProcessesPageModel.ModelView.RunningGames.Count - 1].Start();
                        ManageProcessesPageModel.ModelView.ContentGrid.Children.Add((DataContext as RealMainPageModelView).ProcessModelView.Game.GetManageProcessPage(ManageProcessesPageModel.ModelView.RunningGames[ManageProcessesPageModel.ModelView.RunningGames.Count - 1], APIModel.SettingArgs));
                        MyPageBase From = null;
                        foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
                        {
                            if (item.Visibility == Visibility.Visible)
                            {
                                From = item;
                            }
                        }
                        foreach (MyPageBase item in FindControl.FromTag("ProcessesManagePage", (App.Current.Resources["MainPage"] as Grid)))
                        {
                            item.Show(From);
                        }
                        CancelButton_Click(sender,new RoutedEventArgs());
                    });
                }
            }
            else if (e.PropertyName == "Canceled")
            {
                CancelButton_Click(null,new RoutedEventArgs());
            }
        }

        private void MyComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as RealMainPageModelView).CurretGame=
                ((sender as Controls.MyItemsCardItem).DataContext 
                as Contract.GameInfoBase);
        }
        static ProcessModelView _pmv;
        private void LaunchGame(object sender, RoutedEventArgs e)
        {
            if (APIModel.CurretGame != null)
            {
            try
            {
                    _pmv= new ProcessModelView(APIModel.CurretGame);
                    (DataContext as RealMainPageModelView).ProcessModelView = _pmv;
                    (DataContext as RealMainPageModelView).ProcessModelView.BuildProcess();
            }
                catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(CancelButton.Visibility == Visibility.Hidden)
                {
                    LaunchGame(sender, e);
                }
            }
            else if (e.Key == Key.Escape)
            {
                if (CancelButton.Visibility == Visibility.Visible)
                {
                    CancelButton_Click(sender, e);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                CancelButton.Visibility = Visibility.Hidden;
                LaunchGameBox.Visibility = Visibility.Visible;
                _dnani.From = 80;
                _dnani.To = 0;
                StatuStackPanel.BeginAnimation(HeightProperty, _dnani);
            });
            _pmv.Cancel();
            _pmv = null;
        }

        private void MyButton_Click_1(object sender, RoutedEventArgs e)
        {
            GameInfoCollection.NowYouCanDisposeYourthings = true;
        }
    }

}
