using Avalonia.Controls;
using Avalonia.Threading;
using MEFL.Contract;
using System.Diagnostics;

namespace CLAddIn.Views
{
    public partial class MEFLRealseTypeManage : UserControl,IProcessManagePage
    {
        public MEFLRealseTypeManage()
        {
            InitializeComponent();
        }

        public MEFLRealseTypeManage(Process process)
        {
            InitializeComponent();
            //process.StartInfo.RedirectStandardError = false;
            //process.StartInfo.CreateNoWindow = false;

            //process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.RedirectStandardError= true;
            process.EnableRaisingEvents = true; 
            //process.OutputDataReceived += Process_OutputDataReceived;
            //process.ErrorDataReceived += Process_ErrorDataReceived;
            process.Start();
            //process.BeginOutputReadLine();
            //process.BeginErrorReadLine();
            process.Exited += Process_Exited;
            TB.Text = $"{DateTime.Now} 游戏启动，等待游戏加载";
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(e.Data))
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Scl.ScrollToEnd();
                    TB.Text = TB.Text + "\n" + e.Data;
                });
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Scl.ScrollToEnd();
                TB.Text = TB.Text + "\n" + e.Data;
            });
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            if((sender as Process).ExitCode == 0)
            {
                Exited?.Invoke(this, e);
            }
        }

        public event EventHandler Exited;
    }
}
