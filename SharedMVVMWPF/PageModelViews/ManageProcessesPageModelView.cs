using MEFL.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MEFL.PageModelViews
{
    public class ManageProcessesPageModelView:PageModelViewBase
    {
        private DoubleAnimation _dbani=new DoubleAnimation() { EasingFunction=new PowerEase(),Duration=new Duration(TimeSpan.FromSeconds(0.2))};
        public ManageProcessesPageModelView()
        {
            RunningGames = new ObservableCollection<Process>();
            RunningGames.CollectionChanged += RunningGames_CollectionChanged;
            _ContentGrid = new Grid();
        }

        private void RunningGames_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if((sender as ObservableCollection<Process>).Count > 0)
            {
                LoadButton();
            }
            else
            {
                ReturnToMainPage();
            }
            Invoke("RunningGames");
        }
        private Grid _ContentGrid;

        public Grid ContentGrid
        {
            get { return _ContentGrid; }
            set { _ContentGrid = value; Invoke(nameof(ContentGrid)); }
        }

        public ObservableCollection<Process> RunningGames { get; set; }
        public void LoadButton()
        {
            foreach (ChangePageButton item in FindControl.FromTag("ProcessesManagePage", (App.Current.Resources["ChangePageButtons"] as StackPanel)))
            {
                    _dbani.From = item.Width;
                    _dbani.To = 45.0;
                    item.BeginAnimation(FrameworkElement.WidthProperty, _dbani);
            }
        }
        public void ReturnToMainPage()
        {
            MyPageBase From = new MyPageBase();
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
                if(From.Tag.ToString()== "ProcessesManagePage")
                {
                    item.Show(From);
                }
            }
            From = null;
            foreach (ChangePageButton item in FindControl.FromTag("ProcessesManagePage", (App.Current.Resources["ChangePageButtons"] as StackPanel)))
            {
                _dbani.From = item.Width;
                _dbani.To = 0.0;
                item.BeginAnimation(FrameworkElement.WidthProperty, _dbani);
            }
        }
    }

    public static class ManageProcessesPageModel
    {
        public static ManageProcessesPageModelView ModelView { get; set; }
        static ManageProcessesPageModel()
        {
            ModelView = new ManageProcessesPageModelView();
        }

    }
}
