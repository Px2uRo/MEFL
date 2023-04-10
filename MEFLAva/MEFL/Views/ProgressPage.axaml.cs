using Avalonia.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MEFL.Views
{
    public partial class ProgressPage : UserControl
    {
        public static ObservableCollection<TabItem> TabL;
        public static ProgressPage UI;
        public ProgressPage()
        {
            InitializeComponent();
        }

        static ProgressPage()
        {
            TabL = new ObservableCollection<TabItem>();
            UI = new ProgressPage();
            UI.TabC.Items = TabL;
        }
    }
}
