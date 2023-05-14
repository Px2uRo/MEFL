using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using MEFL.Contract;
using System;

namespace MEFL.AvaControls
{
    public partial class WaringDialog : UserControl,IDialogContent
    {
        static WaringDialog UI = new WaringDialog();
        public WaringDialog()
        {
            InitializeComponent();
            ExitBtn.Click += ExitBtn_Click;
        }

        private void ExitBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this, e);
        }

        public static void Show(string content)
        {
            Dispatcher.UIThread.InvokeAsync(new Action(() =>
            {
                UI.ContentTB.Text = content;
                ContentDialog.Show(UI);
            }));
        }

        public void WindowSizeChanged(Size size)
        {

        }

        public event EventHandler<EventArgs> Quited;
    }
}
