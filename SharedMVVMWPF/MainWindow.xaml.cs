using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
using MEFL.Controls;
using MEFL.PageModelViews;

namespace MEFL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DoubleAnimation _dbani;
        public MainWindow()
        {
#if DEBUG
            try
            {
                (new FileInfo(@"I:\Xiong's\MEFLCollection\ProjectUnite\TestAddIn\TestAddIn\bin\Debug\net6.0-windows\TestAddIn.dll"))
            .CopyTo(@"I:\Xiong's\MEFLCollection\MEFLNet6\bin\Debug\net6.0-windows\AddIns\TestAddIn.dll", true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
#endif
            this.DataContext = this;
            InitializeComponent();
            //PART_Window_Content.DataContext = this;
            _dbani = new DoubleAnimation();
            _dbani.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new Pages.RealMainPage() { Tag = "RealMainPage", Visibility = Visibility.Visible });
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new Pages.SettingPage() { Tag = "SettingPage", Visibility = Visibility.Hidden });
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new Pages.ExtensionPage() { Tag = "ExtensionPage", Visibility = Visibility.Hidden });
            (App.Current.Resources["ChangePageButtons"] as StackPanel).Children.Add(
                new ChangePageButton
                {
                    Width = 45,
                    Tag = "ExtensionPage",
                    Margin = new Thickness(0, 5, 0, 0),
                    Content = new System.Windows.Shapes.Path()
                    {
                        Data = new PathGeometry() { Figures = PathFigureCollection.Parse("M0.500053 0.500053 8.15191 0.500053 8.13135 0.700159C8.13135 2.63976 9.73378 4.21211 11.7105 4.21211 13.6872 4.21211 15.2896 2.63976 15.2896 0.700159L15.2691 0.500053 22.9209 0.500053 22.9209 8.10635 22.9209 8.10635C24.8976 8.10635 26.5001 9.6787 26.5001 11.6183 26.5001 13.5579 24.8976 15.1302 22.9209 15.1302L22.9209 15.1302 22.9209 22.5001 4.23694 22.5001C2.17312 22.5001 0.500053 20.8584 0.500053 18.8333Z") },
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 1.0,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    }
                });
            (App.Current.Resources["ChangePageButtons"] as StackPanel).Children.Add(
                new ChangePageButton
                {
                    Width = 45,
                    Tag = "SettingPage",
                    Margin = new Thickness(0, 5, 0, 0),
                    Content = new System.Windows.Shapes.Path()
                    {
                        Height=23,Width=23,Stretch=Stretch.Fill,
                        Data = new PathGeometry() { Figures = PathFigureCollection.Parse("M0 1.15848 1.01007 2.10614 2.02013 3.0538 3.2831 1.86895 2.30113 0.947658 1.29106 0 3.3111 0 4.29307 0.921299 4.29942 0.915336 4.29942 0.927257 4.32117 0.947658 4.29942 0.96806 4.29942 2.81056 11.9698 10.007 13.9899 10.007 15 10.9547 15 12.85 13.9899 11.9023 12.9799 10.9547 11.7357 12.1219 11.7363 12.1225 11.7552 12.1048 11.7551 12.1048 11.7552 12.1047 12.7525 13.0403 12.7549 13.0381 13.7599 13.995 11.7606 13.995 11.7552 14 11.7499 13.995 11.7499 13.995 11.7499 13.995 10.8066 13.1099 10.7877 13.1276 10.8066 13.1099 10.8064 13.1098 10.7876 13.1275 10.7876 13.092 10.7453 13.0523 10.7876 13.0126 10.7875 11.2794 3.0302 4.00145 1.01007 4.00145 0 3.0538Z") },
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 1.0,
                        HorizontalAlignment = HorizontalAlignment.Center,   
                        VerticalAlignment = VerticalAlignment.Center
                    }
                });
            if (true)
            {
                Application.Current.Resources["I18N_String_Setting_Title_Now"] = Application.Current.Resources["I18N_String_Setting_Default_Title"].ToString();
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border a;
            if (sender is Border)
            {
                a = sender as Border;
                if (a.Name == "MinWindowButton")
                {
                    WindowState = WindowState.Minimized;
                }
                else if (a.Name == "ChangeSizeButton")
                {
                    if (WindowState == WindowState.Normal)
                    {
                        WindowState=WindowState.Maximized;
                    }
                    else
                    {
                        WindowState = WindowState.Normal;
                    }
                }
                else if (a.Name == "CloseWindowsButton")
                {
                    Close();
                }
                else if (a.Tag != null&&a.Tag is String)
                {
                    
                }
            }
            else
            {
                throw new Exception("What's up 你这代码有问题啊");
            }
            a = null;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WinWindowIcon.Visibility = Visibility.Hidden;
                MaxWindowIcon.Visibility = Visibility.Visible;
            }
            else
            {
                WinWindowIcon.Visibility = Visibility.Visible;
                MaxWindowIcon.Visibility = Visibility.Hidden;
            }

        }

        private void MinWindowButton_MouseEnter(object sender, MouseEventArgs e)
        {
            _dbani.From = 0;
            if((sender as Border).Name == "CloseWindowsButton")
            {
                _dbani.To = 1;
            }
            else
            {
                _dbani.To = 0.2;
            }
            (sender as Border).BeginAnimation(OpacityProperty,_dbani);
        }

        private void MinWindowButton_MouseLeave(object sender, MouseEventArgs e)
        {
            _dbani.To = 0;
            if ((sender as Border).Name == "CloseWindowsButton")
            {
                _dbani.From = 1;
            }
            else
            {
                _dbani.From = 0.2;
            }
            (sender as Border).BeginAnimation(OpacityProperty,_dbani);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Ini(object sender, EventArgs e)
        {
            //PageModelViews.ExtraAddInPageModelView.Reload();
        }

    }
}
