using MEFL.Controls;
using System;
using System.Collections.Generic;
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
            MyCB.ItemsSource = new List<string>() { "989"};
        }
    }

    public class ModelView: INotifyPropertyChanged
    {
        public List<String> Foo { get=>Model.Foo; 
            set 
            {
                Model.Foo = value;
            } 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public static class Model
    {
        public static List<String> Foo { get; set; }
        static Model() 
        {
            Foo = new List<String>() { "4", "5", "6" };
        }
    }
}
