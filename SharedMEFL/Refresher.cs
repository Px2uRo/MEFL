using static MEFL.APIData.APIModel;
using MEFL;
using System.Collections.ObjectModel;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;
using MEFL.PageModelViews;
using System.Windows;
#if WPF
using MEFL.Controls;
using System.Windows.Controls;
using System.Windows.Shapes;
#elif AVALONIA
using Avalonia.Controls;
#endif

namespace MEFL
{
    public static class GameRefresher
    {
        static Thread t;
        private static bool _Refreshing;
        public static bool Refreshing { get => _Refreshing; set { 
                _Refreshing = value;
            } }
        public static void RefreshCurrect()
        {
            if (APIData.APIModel.MyFolders[SelectedFolderIndex] == null)
            {
                return;
            }
            if (Refreshing)
            {
                return;
            }
            Refreshing = true; 
            t = new Thread(async () =>
            {
                try
                {
                    await GameLoader.LoadAll(MyFolders[SelectedFolderIndex]);
                    Refreshing = false;
#if WPF
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var item in HostingsToUI.res.Children)
                        {
                            ((item as MyExtensionCard).DataContext as HostingModelView).Invoke("IsRefreshing");
                        }
                    });
#elif AVALONIA
                    //TODO UI 触发
#endif
                }
                catch (Exception ex)
                {
                    Debugger.Logger($"刷新时发现未知错误 {ex.Message} at {ex.Source}");
                    Refreshing = false;
#if WPF
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var item in HostingsToUI.res.Children)
                        {
                            ((item as MyExtensionCard).DataContext as HostingModelView).Invoke("IsRefreshing");
                        }
                    });

#elif AVALONIA
                    //TODO UI 触发
#endif
                }
                return;
            });
            t.Start();
        }
    }

    public static class WebListRefresher
    {
        private static bool _IsRefreshingList;

        public static bool IsRefreshing
        {
            get { return _IsRefreshingList; }
            set { _IsRefreshingList = value; }
        }

#if WPF
        public static MyPageBase SovlePage = new() { Tag="SovlePage",Visibility=System.Windows.Visibility.Hidden};
        
        public static void ShowSovlePage()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                MyPageBase From = null;
                foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
                {
                    if (item.Visibility == Visibility.Visible)
                    {
                        From = item;
                        if (item.Tag as String != From.Tag as String)
                        {
                            item.Hide();
                        }
                    }
                }
                foreach (MyPageBase item in FindControl.FromTag("SovlePage", (App.Current.Resources["MainPage"] as Grid)))
                {
                    item.Show(From);
                }
                From = null;
            });
        }
#elif AVALONIA
        public static UserControl SovlePage = new() { Tag = "SovlePage", IsVisible=false };
#endif

        public static void CleanSovlePage()
        {
            if(SovlePage.DataContext != null)
            {
                GC.SuppressFinalize(SovlePage.Content);
                SovlePage.Content = null;
                (SovlePage.DataContext as Contract.InstallProgressInput).Dispose();
                SovlePage.DataContext = null;
            }
        }


        internal static void GoToDownloadProgressPage()
        {
#if WPF            
App.Current.Dispatcher.Invoke(() =>
            {
                MyPageBase From = null;
                foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
                {
                    if (item.Visibility == Visibility.Visible)
                    {
                        From = item;
                        if (item.Tag as String != From.Tag as String)
                        {
                            item.Hide();
                        }
                    }
                }
                foreach (MyPageBase item in FindControl.FromTag("DownloadingProgressPage", (App.Current.Resources["MainPage"] as Grid)))
                {
                    item.Show(From);
                }
                From = null;
            });
#elif AVALONIA
            //TODO Avalonia 自己的搞法
#endif
        }
    }
}