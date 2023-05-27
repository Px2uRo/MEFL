using Avalonia.Controls;
using Avalonia.Threading;
using MEFL.Contract;
using System.Diagnostics;

namespace ServerInstaller
{
    public partial class ManagePanel : UserControl, IProcessManagePage
    {
        public ManagePanel()
        {
            InitializeComponent();
            SendBtn.Click += SendBtn_Click;
        }

        private void SendBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            _p.StandardInput.WriteLine(Contents.Text);
        }
        Process _p;
        internal ManagePanel(ServerType game, Process process):this()
        {
            MemTB.Text = game.Memory;
            _p = process;
            process.Exited += Process_Exited;
            process.EnableRaisingEvents = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(game.GameJsonPath);
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    ConsoleOutput.Text += $"{e.Data}\n";
                });
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    ConsoleOutput.Text += $"{e.Data}\n";
                });
            }
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ConsoleOutput.Text += "将会在 30 秒后自动退出";
            });
            new Thread(() =>
            {
                Thread.Sleep(30000);
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Exited?.Invoke(this, e);
                });
            }).Start();
        }

        public event EventHandler Exited;
    }
}
