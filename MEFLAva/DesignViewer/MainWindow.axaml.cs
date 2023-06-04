using Avalonia.Controls;
using Avalonia.Input;
using System.Diagnostics;

namespace DesignViewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DragDrop.SetAllowDrop(SearchBar,true);
            SearchBar.AddHandler(DragDrop.DropEvent, (s, e) => {
                Debug.WriteLine("HelloXILU");
            });
        }
    }
}