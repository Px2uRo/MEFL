using Avalonia.Controls;
using Avalonia.Media;

namespace CLAddIn.Views
{
    public partial class ResultHeader : UserControl
    {
        public ResultHeader()
        {
            InitializeComponent();
        }

        public ResultHeader(string title):this()
        {
            TitleTB.Text = title;
            Btn.PointerPressed += Btn_PointerPressed;
        }

        private void Btn_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            var pr = (this.DataContext as TabItem).Parent as TabControl;
            (pr.Items as IList<Object>).Remove(this.DataContext);
        }
    }
}
