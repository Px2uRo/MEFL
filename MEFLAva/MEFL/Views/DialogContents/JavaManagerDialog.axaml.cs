using Avalonia.Controls;
using Avalonia.Threading;
using MEFL.AvaControls;
using MEFL.Contract;
using System;
using System.Threading.Tasks;

namespace MEFL.Views.DialogContents
{
    public partial class JavaManagerDialog : UserControl,IDialogContent
    {
        static JavaManagerDialog UI = new JavaManagerDialog();
        public JavaManagerDialog()
        {
            InitializeComponent();
            ForceBtn.Click += ForceBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
            ApplyBtn.Click += ApplyBtn_Click;
        }

        public static void Show(string Info)
        {
            Dispatcher.UIThread.InvokeAsync(new Action(() =>
            {
                UI.InfoTB.Text = Info;
                ContentDialog.Show(UI);
            }));
            while (true)
            {

            }
        }

        private void ForceBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this, e);
        }

        private void ApplyBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

        }

        public event EventHandler<EventArgs> Quited;
    }
}
