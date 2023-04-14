using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
#if WPF
using MEFL.Controls;
using System.Windows.Controls;
using System.Windows.Media.Animation;
#elif AVALONIA
using Avalonia.Controls;
using Avalonia.Threading;
using MEFL.Views;

#endif

namespace MEFL.PageModelViews
{

    public class DownloadProgressPageModelView:PageModelViewBase
    {
#if WPF
        private DoubleAnimation _dbani=new DoubleAnimation() { EasingFunction=new PowerEase(),Duration=new Duration(TimeSpan.FromSeconds(0.2))};
#elif AVALONIA
        static Button btn = new Button() { Width = 30.0, Height = 30.0 };
#endif
        public DownloadProgressPageModelView()
        {
            DownloadingProgresses = new ();
            DownloadingProgresses.CollectionChanged += DownloadingProgresses_CollectionChanged;
            _ContentGrid = new();
        }

        private void DownloadingProgresses_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if((sender as ObservableCollection<Contract.DownloadProgress>).Count > 0)
            {
                LoadButton();
            }
            else
            {
                ReturnToMainPage();
            }
            Invoke(nameof(DownloadingProgresses));
        }
        private StackPanel _ContentGrid;

        public StackPanel ContentGrid
        {
            get { return _ContentGrid; }
            set { _ContentGrid = value; Invoke(nameof(ContentGrid)); }
        }

        public DownloadProgressCollection DownloadingProgresses { get; set; }
        public void LoadButton()
        {
#if WPF
            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (ChangePageButton item in FindControl.FromTag("DownloadingProgressPage", (App.Current.Resources["ChangePageButtons"] as StackPanel)))
                {
                    _dbani.From = item.Width;
                    _dbani.To = 45.0;
                    item.BeginAnimation(FrameworkElement.WidthProperty, _dbani);
                }
            });
#elif AVALONIA
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                MainWindow.AddTempButton(btn, DownloadProcessPage.UI);
            });
#endif
            
        }
        public void ReturnToMainPage()
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
                foreach (MyPageBase item in FindControl.FromTag("RealMainPage", (App.Current.Resources["MainPage"] as Grid)))
                {
                    if (From.Tag.ToString() == "DownloadingProgressPage")
                    {
                        item.Show(From);
                    }
                }
                From = null;
                foreach (ChangePageButton item in FindControl.FromTag("DownloadingProgressPage", (App.Current.Resources["ChangePageButtons"] as StackPanel)))
                {
                    _dbani.From = item.Width;
                    _dbani.To = 0.0;
                    item.BeginAnimation(FrameworkElement.WidthProperty, _dbani);
                }
            });
#elif AVALONIA
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    MainWindow.RemoveTempButton(btn);
                });
            });
#endif
        }
    }

    public static class DownloadingProgressPageModel
    {
        public static DownloadProgressPageModelView ModelView = new();
    }
}
