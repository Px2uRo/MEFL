using MEFL.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MEFL.Controls;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;

namespace WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void MyComboBox_Initialized(object sender, EventArgs e)
        {

        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ModelView).Obs = new ObservableCollection<MyPair>() { new MyPair() { RealName = "112", Value = "22asddas231" }, new MyPair() { RealName = "11asd4514wqewqsda", Value = "191adsdasd981asdsda0" } };
        }

        private void MyItemsCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Changed");
        }

        private void MyButton_Click_1(object sender, RoutedEventArgs e)
        {
            var mb = MyMessageBox.Show("asa");
        }
    }

    public class ModelView: INotifyPropertyChanged
    {
        private ObservableCollection<MyPair> _obs;
        public  ObservableCollection<MyPair> Obs {
            get 
            {
                return _obs;
            }
            set 
            {
                _obs =value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Obs"));
            }
                
        }

        private int _LangIndex;

        public int LangIndex
        {
            get { 
                return _LangIndex; 
            }
            set {
                if (value != 0)
                {
                    ResourceDictionary dic = new ResourceDictionary();
                    dic.Source = new Uri(@"I:\Xiong's\MEFLCollection\SharedMVVMWPF\I18N\en_US.xaml");
                    foreach (DictionaryEntry item in dic)
                    {
                        App.Current.Resources[item.Key] = item.Value;
                    }
                    dic = null;
                }
                else
                {
                    ResourceDictionary dic = new ResourceDictionary();
                    dic.Source = new Uri(@"I:\Xiong's\MEFLCollection\SharedMVVMWPF\I18N\zh_CN.xaml");
                    foreach (DictionaryEntry item in dic)
                    {
                        App.Current.Resources[item.Key] = item.Value;
                    }
                    dic = null;
                }
                _LangIndex = value;
            }
        }
        public ModelView()
        {
            LangIndex = 0;
            Obs = new ObservableCollection<MyPair>() { new MyPair() { RealName="112",Value="22231"},new MyPair() { RealName = "114514", Value = "1919810" } };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class MyPair
    {
        public string RealName { get; set; }
        public string Value { get; set; }
        public object Icon { get; set; }
    }

    public static class Model
    {

    }
}
