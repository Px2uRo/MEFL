using Avalonia.Controls;
using MEFL.APIData;

namespace MEFL.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var HELLO = this.FindControl<TextBlock>("HELLO");
            HELLO.Text = APIModel.MaxMemory.ToString();
        }
    }
}