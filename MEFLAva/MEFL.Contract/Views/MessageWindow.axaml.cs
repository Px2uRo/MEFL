using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;

namespace MEFL.Contract.Views
{
    public struct MyMessageResult
    {
        public bool HasValue { get; set; }
        public MessageBoxResult Result { get; set; }
        public bool[] CheckBox { get; set; }
        public MyMessageResult()
        {
            HasValue = false;
            Result = MessageBoxResult.None;
            CheckBox = new bool[0];
        }
    }

    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            InitializeComponent();
        }


        public MyMessageResult Result = new();
        private MessageBoxButton _but;
        private bool _actLike = false;

        public bool Chose = false;
        public bool IsClosed { get; set; }
        public MessageWindow(string message, string title, MessageBoxButton button, MyCheckBoxInput[] checkBoxs, bool actsLikeRadioButtons) : this()
        {
            IsClosed = false;
            _actLike = actsLikeRadioButtons;
            InitializeComponent();
            _but = button;
            this.MyTB.Text = message;
            this.Title = title;
            if (button == MessageBoxButton.OK)
            {
                R1.Content = new TextBlock { /*Foreground = new SolidColorBrush(Colors.Green),*/ Text = "好" };
                //R1.Background = new SolidColorBrush(Colors.Green);
                R1.Click += R1_Click;
                MyButtons.Children.Remove(R2);
                MyButtons.Children.Remove(R3);
            }
            else if (button == MessageBoxButton.OKCancel)
            {
                R1.Content = new TextBlock { Text = "取消", Foreground = new SolidColorBrush(Colors.White) };
                R1.Click += R1_Click;
                R1.Background = new SolidColorBrush(Colors.Red);
                R2.Content = new TextBlock { /*Foreground = new SolidColorBrush(Colors.Green),*/ Text = "好" };
                //R2.Background = new SolidColorBrush(Colors.Green);
                R2.Click += R2_Click;
                MyButtons.Children.Remove(R3);
            }
            else if (button == MessageBoxButton.YesNoCancel)
            {
                R1.Content = new TextBlock { Text = "取消" };
                R1.Click += R1_Click;
                R2.Content = new TextBlock { Foreground = new SolidColorBrush(Colors.White), Text = "不" };
                R2.Background = new SolidColorBrush(Colors.Red);
                R2.Click += R2_Click;
                R3.Content = new TextBlock { /*Foreground = new SolidColorBrush(Colors.Green),*/ Text = "好" };
                //R3.Background = new SolidColorBrush(Colors.Green);
                R3.Click += R3_Click;
            }
            else if (button == MessageBoxButton.YesNo)
            {
                R1.Content = new TextBlock { Foreground = new SolidColorBrush(Colors.White), Text = "不" };
                R1.Click += R1_Click;
                R1.Background = new SolidColorBrush(Colors.Red);
                R2.Content = new TextBlock { /*Foreground = new SolidColorBrush(Colors.Green),*/ Text = "好" };
                //R2.Background = new SolidColorBrush(Colors.Green);
                R2.Click += R2_Click;
                MyButtons.Children.Remove(R3);
            }
            Result.CheckBox = new bool[checkBoxs.Length];
            for (int i = 0; i < checkBoxs.Length; i++)
            {
                var item = checkBoxs[i];
                var element = new CheckBox() { Tag = i, Content = item.Content, IsChecked = item.IsChecked, Foreground = new SolidColorBrush(item.Color) };
                Result.CheckBox[i] = item.IsChecked;
                element.Checked += Element_Checked;
                element.Unchecked += Element_Unchecked;
                MyCheckBoxs.Children.Add(element);
            }
            Closed += MessageWindow_Closed;
        }

        private void MessageWindow_Closed(object? sender, EventArgs e)
        {
            IsClosed = true;
        }

        private void Element_Unchecked(object sender, RoutedEventArgs e)
        {
            int index = (int)((sender as CheckBox).Tag);
            Result.CheckBox[index] = false;
        }

        private void Element_Checked(object sender, RoutedEventArgs e)
        {
            int index = (int)((sender as CheckBox).Tag);
            if (_actLike)
            {
                var parent = (sender as CheckBox).Parent as StackPanel;
                foreach (CheckBox item in parent.Children)
                {
                    item.IsChecked = false;
                }
                (sender as CheckBox).Checked -= Element_Checked;
                (sender as CheckBox).IsChecked = true;
                (sender as CheckBox).Checked += Element_Checked;
            }
            Result.CheckBox[index] = true;
        }

        private void R3_Click(object sender, RoutedEventArgs e)
        {
            Result.Result = MessageBoxResult.Yes;
            Result.HasValue = true;
            Close();
        }

        private void R1_Click(object sender, RoutedEventArgs e)
        {
            if (_but == MessageBoxButton.OK)
            {
                Result.Result = MessageBoxResult.OK;
            }
            else if (_but == MessageBoxButton.YesNo)
            {
                Result.Result = MessageBoxResult.No;
            }
            else
            {
                Result.Result = MessageBoxResult.Cancel;
            }
            Result.HasValue = true;
            Close();
        }

        private void R2_Click(object sender, RoutedEventArgs e)
        {

            Result.HasValue = true;
            if (_but == MessageBoxButton.YesNoCancel)
            {
                Result.Result = MessageBoxResult.No;
            }
            else
            {
                Result.Result = MessageBoxResult.Yes;
            }
            Close();
        }

        //MessageBox.Show(message,title,Button,icon,,,)
        public static MyMessageResult Show(string message)
        {
            return Show(message, string.Empty, MessageBoxButton.OK, new MyCheckBoxInput[0], false);
        }
        public static MyMessageResult Show(string message, string title)
        {
            return Show(message, title, MessageBoxButton.OK, new MyCheckBoxInput[0], false);
        }
        public static MyMessageResult Show(string message, string title, MessageBoxButton button)
        {
            return Show(message, title, button, new MyCheckBoxInput[0], false);
        }
        public static MyMessageResult Show(string message, string title, MessageBoxButton button, MyCheckBoxInput[] checkBoxs, bool actsLikeRadioButtons)
        {
            MessageWindow mw = null;
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                mw = new MessageWindow(message, title, button, checkBoxs, actsLikeRadioButtons);
                mw.ShowAsDialog();
            });
            while (mw==null)
            {
                Thread.Sleep(100);
            }
            while (!mw.IsClosed)
            {
                Thread.Sleep(100);
            }
            return mw.Result;
        }

        private void ShowAsDialog()
        {
            var app = Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime;
            ShowDialog(app.MainWindow);
        }

        public static MyMessageResult Show(string message, string title, MessageBoxButton button, MyCheckBoxInput[] checkBoxs)
        {
            return Show(message, title, button, checkBoxs, false);
        }
    }

    #region Enums
    public enum MessageBoxResult
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Yes = 6,
        No = 7
    }

    public struct MyCheckBoxInput
    {
        public string Content;
        public bool IsChecked;
        public Color Color;
        public MyCheckBoxInput(string content, bool isChecked, Color color)
        {
            Content = content;
            IsChecked = isChecked;
            Color = color;
        }
    }


    public enum MessageBoxButton
    {
        OK = 0,
        OKCancel = 1,
        YesNoCancel = 3,
        YesNo = 4
    }
    #endregion
}
