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
            #region 几个按钮
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new Pages.RealMainPage() { Tag = "RealMainPage", Visibility = Visibility.Visible });
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new Pages.SettingPage() { Tag = "SettingPage", Visibility = Visibility.Hidden });
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new Pages.ExtensionPage() { Tag = "ExtensionPage", Visibility = Visibility.Hidden });
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new Pages.UserManagePage() { Tag = "UserManagePage", Visibility = Visibility.Hidden });
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new Pages.DownloadPage() { Tag = "DownloadPage", Visibility = Visibility.Hidden });
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new SpecialPages.ManageProcessesPage() { Tag = "ProcessesManagePage", Visibility = Visibility.Hidden });

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
                        Height = 23,
                        Width = 23,
                        Stretch = Stretch.Fill,
                        Data = new PathGeometry() { Figures = PathFigureCollection.Parse("M0 1.15848 1.01007 2.10614 2.02013 3.0538 3.2831 1.86895 2.30113 0.947658 1.29106 0 3.3111 0 4.29307 0.921299 4.29942 0.915336 4.29942 0.927257 4.32117 0.947658 4.29942 0.96806 4.29942 2.81056 11.9698 10.007 13.9899 10.007 15 10.9547 15 12.85 13.9899 11.9023 12.9799 10.9547 11.7357 12.1219 11.7363 12.1225 11.7552 12.1048 11.7551 12.1048 11.7552 12.1047 12.7525 13.0403 12.7549 13.0381 13.7599 13.995 11.7606 13.995 11.7552 14 11.7499 13.995 11.7499 13.995 11.7499 13.995 10.8066 13.1099 10.7877 13.1276 10.8066 13.1099 10.8064 13.1098 10.7876 13.1275 10.7876 13.092 10.7453 13.0523 10.7876 13.0126 10.7875 11.2794 3.0302 4.00145 1.01007 4.00145 0 3.0538Z") },
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
                    Tag = "UserManagePage",
                    Margin = new Thickness(0, 5, 0, 0),
                    Content = new System.Windows.Shapes.Path()
                    {
                        Height = 23,
                        Width = 23,
                        Stretch = Stretch.Fill,
                        Data = new PathGeometry() { Figures = PathFigureCollection.Parse("M1.50007 26.9416C1.5067 27.0771 1.51331 27.2126 1.51995 27.3481L1.51996 27.3481 1.51996 27.3481 1.50007 27.3481 1.50007 26.9416ZM27.5001 26.9413 27.5001 27.3481 27.4802 27.3481 27.5001 26.9413ZM14.5 13.5173C21.6797 13.5173 27.5 19.5274 27.5 26.9413L27.4801 27.3481C16.6192 34.44 10.1734 27.3481 1.51996 27.3481L1.50007 26.9413 1.50007 26.9416C1.50007 26.9415 1.50005 26.9414 1.50005 26.9413 1.50005 19.5274 7.32034 13.5173 14.5 13.5173ZM14.5 0.500053C17.9811 0.500053 20.8031 3.41405 20.8031 7.00867 20.8031 10.6033 17.9811 13.5173 14.5 13.5173 11.019 13.5173 8.19702 10.6033 8.19702 7.00867 8.19702 3.41405 11.019 0.500053 14.5 0.500053Z") },
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
                    Tag = "DownloadPage",
                    Margin = new Thickness(0, 5, 0, 0),
                    Content = new System.Windows.Shapes.Path()
                    {
                        Height = 26,
                        Width = 25,
                        Stretch = Stretch.Fill,
                        Data = new PathGeometry() { Figures = PathFigureCollection.Parse("M184.399 0.500053 198.5 5.93756 198.5 22.509 191.364 25.9971 191.372 25.9515 191.372 25.3676C191.372 25.2007 191.458 25.0756 191.63 24.9922L191.931 24.867C192.103 24.7836 192.231 24.575 192.145 24.3665L191.845 23.6157C191.802 23.5322 191.759 23.4071 191.673 23.3237L189.998 20.8627C189.783 20.529 189.225 20.6541 189.225 21.0713L189.225 21.3632C189.225 21.6135 189.053 21.7804 188.795 21.7804L188.623 21.7804C188.452 21.7804 188.323 21.6969 188.237 21.5301L187.893 20.821C187.807 20.675 187.657 20.602 187.507 20.602 187.356 20.602 187.206 20.675 187.12 20.821L186.776 21.1964C186.691 21.2798 186.562 21.3215 186.476 21.3215L186.175 21.3215C185.96 21.3215 185.746 21.405 185.574 21.5718L183.856 22.9066C183.512 23.1986 183.426 23.6991 183.684 24.0328L184.801 25.8264C185.101 26.2852 185.746 26.3686 186.132 25.9932L186.948 25.2007C187.249 24.9087 187.764 24.867 188.108 25.159L189.525 26.2852C189.611 26.3269 189.697 26.3686 189.783 26.3686L190.604 26.3686 184.198 29.5001 170.5 22.3795 170.5 5.41974 184.399 0.500053 171.997 4.92215 171.997 4.92217 170.5 5.45587 170.5 6.98444 170.5 6.98445 170.5 13.7699 170.809 13.5267C170.927 13.4693 171.056 13.438 171.185 13.438L172.645 13.438C172.817 13.438 172.946 13.5215 173.032 13.6883L174.578 16.6915C174.621 16.775 174.664 16.8167 174.707 16.8584L175.222 17.1921C175.523 17.3589 175.91 17.1921 175.91 16.8584L175.91 15.4819C175.91 15.2733 175.996 15.0648 176.167 14.8979L177.284 13.8134C177.456 13.6466 177.757 13.6466 177.928 13.8552L179.217 15.4402C179.303 15.5236 179.432 15.607 179.561 15.607L179.861 15.607C180.076 15.607 180.291 15.6905 180.463 15.8573L180.806 16.191C180.978 16.3579 181.193 16.4413 181.408 16.4413L181.493 16.4413C181.751 16.4413 181.923 16.2744 181.923 16.0242L181.923 15.0231C181.923 14.8562 181.837 14.7311 181.665 14.6477L181.279 14.4808C181.15 14.3974 181.021 14.2723 181.021 14.1054L181.021 13.438C181.021 13.1878 181.193 13.0209 181.45 13.0209L182.524 13.0209C182.653 13.0209 182.782 12.9375 182.868 12.8541L183.469 11.9781C183.555 11.853 183.598 11.6861 183.598 11.5193L183.598 10.6434C183.598 10.5599 183.598 10.5182 183.555 10.4348L183.254 9.18346C183.211 8.9749 183.297 8.80806 183.469 8.72463L183.684 8.64121C183.899 8.55778 184.113 8.59949 184.242 8.76634 184.242 8.76634 184.629 9.55886 184.887 9.7257 184.973 9.76742 185.23 9.80913 185.316 9.7257 185.574 9.47544 185.488 8.64121 185.402 8.09896 185.359 7.80697 185.488 7.51499 185.746 7.30643L186.862 6.47221C186.991 6.38878 187.034 6.26364 187.034 6.13851L187.034 5.92996C187.034 5.67968 187.206 5.51283 187.464 5.51283 187.721 5.51283 187.893 5.34599 187.893 5.09572L187.893 5.0123C187.893 4.80374 187.764 4.6369 187.55 4.59518L186.776 4.42833C186.476 4.34491 186.347 4.01122 186.519 3.76095L187.034 3.09356C187.292 2.75987 187.678 2.59302 188.065 2.59302L189.397 2.59302C189.482 2.59302 189.566 2.57999 189.644 2.55588L189.666 2.54575 184.399 0.500053ZM187.721 7.95298C187.625 7.95298 187.528 7.97383 187.507 8.01555 187.335 8.76635 187.163 9.39202 185.359 10.5182 185.273 10.6017 185.617 11.0188 185.789 10.9354 186.948 10.1845 188.28 9.3086 187.936 8.01555 187.915 7.97383 187.818 7.95298 187.721 7.95298ZM183.636 16.9523C183.534 16.9418 183.426 16.9627 183.34 17.0252L182.481 17.6509C182.224 17.8178 182.267 18.2349 182.567 18.36L183.169 18.652C183.426 18.7354 183.684 18.6103 183.77 18.36L184.028 17.4424C184.071 17.3172 184.028 17.1504 183.899 17.067 183.834 17.0044 183.738 16.9627 183.636 16.9523ZM187.077 17.6092 187.077 18.0263 188.151 19.4028C188.28 19.5697 188.495 19.6114 188.666 19.5279L189.225 19.2777 189.955 19.5279C190.213 19.6114 190.513 19.4028 190.513 19.1525 190.513 18.9857 190.427 18.8606 190.299 18.7771L188.452 17.6926C188.409 17.6092 188.323 17.6092 188.237 17.6092L187.077 17.6092ZM179.367 17.3277C179.271 17.3381 179.174 17.3798 179.088 17.4424L178.916 17.6092 181.45 19.6531C181.493 19.6948 181.536 19.6948 181.579 19.7365L182.524 20.0285C182.696 20.0702 182.868 20.0285 182.954 19.9451L183.211 19.6948C181.408 19.1942 179.646 17.4007 179.646 17.4007 179.561 17.3381 179.464 17.3172 179.367 17.3277Z") },
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 1.0,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    }
                });
            (App.Current.Resources["ChangePageButtons"] as StackPanel).Children.Add(
                new ChangePageButton
    {
        Width = 0,
        Tag = "ProcessesManagePage",
        Margin = new Thickness(0, 5, 0, 0),
        Content = new System.Windows.Shapes.Path()
        {
            Height = 23,
            Width = 23,
            Stretch = Stretch.Fill,
            Data = new PathGeometry() { Figures = PathFigureCollection.Parse("M29.5001 0.500053 58.5001 0.500053 58.5001 30.5001 29.5001 30.5001 29.5001 0.500053ZM37.3407 3.94282 37.3407 8.56539 32.8722 8.56539 32.8722 12.9949 37.3407 12.9949 37.3407 17.6175 41.6226 17.6175 41.6226 12.9949 46.0911 12.9949 46.0911 8.56539 41.6226 8.56539 41.6226 3.94282 37.3407 3.94282ZM48.2649 14.2295C47.1414 14.2295 46.2306 15.1717 46.2306 16.3339 46.2306 17.4961 47.1414 18.4382 48.2649 18.4382 49.3883 18.4382 50.2991 17.4961 50.2991 16.3339 50.2991 15.1717 49.3883 14.2295 48.2649 14.2295ZM52.4383 18.2504C51.3148 18.2504 50.4041 19.1926 50.4041 20.3548 50.4041 21.517 51.3148 22.4591 52.4383 22.4591 53.5617 22.4591 54.4725 21.517 54.4725 20.3548 54.4725 19.1926 53.5617 18.2504 52.4383 18.2504ZM43.8615 18.2504C42.738 18.2504 41.8273 19.1926 41.8273 20.3548 41.8273 21.517 42.738 22.4591 43.8615 22.4591 44.9849 22.4591 45.8957 21.517 45.8957 20.3548 45.8957 19.1926 44.9849 18.2504 43.8615 18.2504ZM48.2649 22.85C47.1414 22.85 46.2306 23.7921 46.2306 24.9543 46.2306 26.1165 47.1414 27.0587 48.2649 27.0587 49.3883 27.0587 50.2991 26.1165 50.2991 24.9543 50.2991 23.7921 49.3883 22.85 48.2649 22.85Z") },
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 1.0,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        }
    });
            #endregion
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
                    App.Current.Shutdown();
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
