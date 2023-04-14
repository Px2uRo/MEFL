using Avalonia.Controls;
using Avalonia.Threading;
using MEFL.AvaControls;
using MEFL.Contract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MEFL.Views.DialogContents
{
    public partial class JavaManagerDialog : UserControl,IDialogContent
    {
        public int Action = -1;
        static JavaManagerDialog UI = new JavaManagerDialog();
        public JavaManagerDialog()
        {
            InitializeComponent();
            ForceBtn.Click += ForceBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
            ApplyBtn.Click += ApplyBtn_Click;
        }

        public static bool Show(string Info,out int action)
        {
            Dispatcher.UIThread.InvokeAsync(new Action(() =>
            {
                ContentDialog.Show(UI); 
                UI.InfoTB.Text = Info;
                UI.Action = -1;
            }));
            Thread.Sleep(300);
            while (UI.Action==-1)
            {

            }
            action = UI.Action;
            if (UI.Action == 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ForceBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Action = 0;
        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this, e);
        }

        private void ApplyBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Action= 1;
        }

        public event EventHandler<EventArgs> Quited;
    }
}
