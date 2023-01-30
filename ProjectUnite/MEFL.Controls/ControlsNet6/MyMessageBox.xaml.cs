using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MEFL.Controls
{
    public struct MyMessageResult
    {
        public bool HasValue { get; set; }
        public MessageBoxResult Result { get; set; }
        public bool[] CheckBox { get; set; }
        public MyMessageResult()
        {
            HasValue= false;
            Result = MessageBoxResult.None;
            CheckBox= new bool[0];
        }
    }

    public struct MyCheckBoxInput
    {
        public string Content;
        public bool IsChecked;
        public Color Color;
        public MyCheckBoxInput(string content,bool isChecked,Color color) 
        { 
            Content= content;
            IsChecked=isChecked;
            Color=color;
        }
    }
    /// <summary>
    /// MyMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyMessageBox : Window
    {
        public MyMessageResult Result = new();
        private MessageBoxButton _but;

        protected override void OnClosed(EventArgs e)
        {
            Result.HasValue= true;
            base.OnClosed(e);
            GC.SuppressFinalize(this);
        }

        public MyMessageBox(string message,string title, MessageBoxButton button,MyCheckBoxInput[] checkBoxs)
        {
            InitializeComponent();
            _but= button;
            this.MyTB.Text= message;
            this.Title=title;
            if (button == MessageBoxButton.OK)
            {
                R1.Content = "好。";
                R1.BorderBrush = new SolidColorBrush(Colors.Green);
                R1.Click += R1_Click;
                MyButtons.Children.Remove(R2);
                MyButtons.Children.Remove(R3);
            }
            else if (button == MessageBoxButton.OKCancel)
            {
                R1.Content = "取消。";
                R1.Click += R1_Click;
                R1.BorderBrush = new SolidColorBrush(Colors.Red);
                R2.Content = "好。";
                R2.BorderBrush = new SolidColorBrush(Colors.Green);
                R2.Click += R2_Click;
                MyButtons.Children.Remove(R3);
            }
            else if(button == MessageBoxButton.YesNoCancel)
            {
                R1.Content = "取消";
                R1.BorderBrush = new SolidColorBrush(Colors.Red);
                R1.Click += R1_Click;
                R2.Content = "不。";
                R2.Click += R2_Click; 
                R3.Content = "好。";
                R3.BorderBrush = new SolidColorBrush(Colors.Green);
                R3.Click += R3_Click;
            }
            else if (button == MessageBoxButton.YesNo)
            {
                R1.Content = "不。";
                R1.Click += R1_Click;
                R1.BorderBrush = new SolidColorBrush(Colors.Red);
                R2.Content = "好。";
                R2.BorderBrush = new SolidColorBrush(Colors.Green);
                R2.Click += R2_Click;
                MyButtons.Children.Remove(R3);
            }
            Result.CheckBox = new bool[checkBoxs.Length];
            for (int i = 0; i < checkBoxs.Length; i++)
            {
                var item = checkBoxs[i];
                var element = new CheckBox() {Tag=i,Content=item.Content,IsChecked=item.IsChecked,Foreground=new SolidColorBrush(item.Color)};
                Result.CheckBox[i] = item.IsChecked;
                element.Checked += Element_Checked;
                element.Unchecked += Element_Unchecked;
                MyCheckBoxs.Children.Add(element);
            }
        }

        private void Element_Unchecked(object sender, RoutedEventArgs e)
        {
            int index = (int)((sender as CheckBox).Tag);
            Result.CheckBox[index] = false;
        }

        private void Element_Checked(object sender, RoutedEventArgs e)
        {
            int index = (int)((sender as CheckBox).Tag);
            Result.CheckBox[index] = true;
        }

        private void R3_Click(object sender, RoutedEventArgs e)
        {
            Result.Result= MessageBoxResult.Yes;
            Result.HasValue= true;
            this.Close();
        }

        private void R1_Click(object sender, RoutedEventArgs e)
        {
            if (_but == MessageBoxButton.OK)
            {
                Result.Result= MessageBoxResult.OK;
            }
            else if(_but == MessageBoxButton.YesNo)
            {
                Result.Result= MessageBoxResult.No;
            }
            else
            {
                Result.Result= MessageBoxResult.Cancel;
            }
            Result.HasValue= true;
            this.Close();
        }

        private void R2_Click(object sender, RoutedEventArgs e)
        {

            Result.HasValue = true;
            if (_but == MessageBoxButton.YesNoCancel)
            {
                Result.Result= MessageBoxResult.No;
            }
            else { 
                Result.Result= MessageBoxResult.Yes;
            }
            this.Close();
        }

        //MessageBox.Show(message,title,Button,icon,,,)
        public static MyMessageBox Show(string message)
        {
            return Show(message, string.Empty,MessageBoxButton.OK);
        }
        public static MyMessageBox Show(string message,string title)
        {
            return Show(message, title, MessageBoxButton.OK);
        }
        public static MyMessageBox Show(string message, string title, MessageBoxButton button)
        {
            return Show(message, title, button, new MyCheckBoxInput[0]);
        }
        public static MyMessageBox Show(string message,string title,MessageBoxButton button, MyCheckBoxInput[] checkBoxs)
        {
            MyMessageBox mb = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                mb = new MyMessageBox(message, title, button, checkBoxs);
                mb.Show();
            });
            return mb;
        }
    }
}
