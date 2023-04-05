using Avalonia.Controls;
using MEFL.PageModelViews;

namespace MEFL.Views
{
    public partial class SettingPage : UserControl
    {
        internal static IControl UI = new SettingPage();

        public SettingPage()
        {
            InitializeComponent();
            this.DataContext = new SettingPageModelView();
        }
    }
}
