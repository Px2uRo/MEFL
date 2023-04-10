using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using MEFL.Contract;
using MEFL.Views;
using System.Linq;
using Avalonia.Threading;
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

    public class RunningGamesCollection: Dictionary<Process, IProcessManagePage>
    {
        Button btn = new Button() { Width = 30.0, Height = 30.0 };
        public void AddItem(Process process,IProcessManagePage page,GameInfoBase game)
        {
            page.Exited += Page_Exited;
            Add(process,page);
            LoadButton();
#if AVALONIA
            var tabItem = new TabItem() { Content=page,Header=game.Name};
            ProgressPage.TabL.Add(tabItem);
#elif WPF

#endif
            ManageProcessesPageModel.ModelView.Invoke("RunningGames");
        }

        public void RemoveItem(IProcessManagePage page)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var lq2 = ProgressPage.TabL.Where((x) => x.Content == page).ToArray();
                ProgressPage.TabL.Remove(lq2[0]);
            });
            var lq = this.Where((x) => x.Value == page).ToArray();
            Remove(lq[0].Key);
            if (Count == 0)
            {
                ReturnToMainPage();
            }
        }

        private void Page_Exited(object? sender, EventArgs e)
        {
            RemoveItem(sender as IProcessManagePage);
        }

        private void ReturnToMainPage()
        {
            MainWindow.RemoveTempButton(btn);
        }

        private void LoadButton()
        {
#if AVALONIA
            MainWindow.AddTempButton(btn, ProgressPage.UI);
#elif WPF

#endif
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
