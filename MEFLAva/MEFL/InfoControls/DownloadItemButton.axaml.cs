using Avalonia;
using Avalonia.Controls;
using MEFL.Contract;

namespace MEFL.InfoControls
{
    public partial class DownloadItemButton : UserControl
    {
        public DownloadItemButton()
        {
            InitializeComponent();
            Excutebtn.Click += Excutebtn_Click;
            MoreOptionGrid.PointerEnter += MoreOptionGrid_PointerEnter;
            MoreOptionGrid.PointerLeave += MoreOptionGrid_PointerLeave;
        }

        private void MoreOptionGrid_PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.MarginBack(new Thickness(30, 0, 0, 0)).RunAsync(MoreOptionBtn, null);
            Animations.WidthBack(288.0).RunAsync(Excutebtn, null);
        }

        private void MoreOptionGrid_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Animations.MarginGo(new(0, 0, 0, 0)).RunAsync(MoreOptionBtn, null);
            Animations.WidthGo(258.0).RunAsync(Excutebtn, null);
        }

        private void Excutebtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

        }

        public DownloadItemButton(LauncherWebVersionInfo info):this()
        {
            this.DataContext= info;
        }
    }
}
