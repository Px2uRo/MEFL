using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using MEFL.Contract;
#if WPF
using MEFL.Controls;
using System.Windows.Controls;
using System.Windows.Media.Animation;
#elif AVALONIA
using Avalonia.Controls;
using Avalonia.Animation;
#endif

namespace MEFL.PageModelViews
{
    public class ManageProcessesPageModelView:PageModelViewBase
    {
#if WPF
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

                private DoubleAnimation _dbani=new DoubleAnimation() { EasingFunction=new PowerEase(),Duration=new Duration(TimeSpan.FromSeconds(0.2))};

#elif AVALONIA
        private void ReturnToMainPage()
        {

        }

        private void LoadButton()
        {

        }
#endif
        public ManageProcessesPageModelView()
        {
            RunningGames = new();
            _ContentGrid = new Grid();
        }

        private Grid _ContentGrid;

        public Grid ContentGrid
        {
            get { return _ContentGrid; }
            set { _ContentGrid = value; Invoke(nameof(ContentGrid)); }
        }

        public RunningGamesCollection RunningGames { get; set; }
    }

    public class RunningGamesCollection: Dictionary<Process, IManageAccountPage>
    {
        public void AddItem(Process process,IManageAccountPage page)
        {
            this.Add(process,page);
            if (this.Count > 0)
            {
                //LoadButton();
            }
            else
            {
                //ReturnToMainPage();
            }
            ManageProcessesPageModel.ModelView.Invoke("RunningGames");
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
