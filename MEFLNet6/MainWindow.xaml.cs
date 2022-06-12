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
    public class Convertest : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine(value);
            if (value as String == parameter as String)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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
                (new FileInfo(@"D:\Xiong's\WPF\TestAddIn\TestAddIn\bin\Debug\net6.0-windows\TestAddIn.dll"))
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
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new Pages.SettingPage() { Tag= "SettingPage" });
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new Pages.ExtensionPage() { Tag = "ExtensionPage" });
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

        private void TextBlock_Initialized(object sender, EventArgs e)
        {

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

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        public static Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ControlModel.TimeMultiple = 5;
        }

        private void Ini(object sender, EventArgs e)
        {
            ((sender as StackPanel).DataContext as ExtraAddInPageModelView).Ini(sender, e);
        }
    }
}
