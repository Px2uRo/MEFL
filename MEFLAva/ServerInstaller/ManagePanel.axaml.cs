using Avalonia.Controls;
using Avalonia.Media;
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
            this.KeyDown += ManagePanel_KeyDown;
        }

        private void ManagePanel_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Enter)
            {
                SendBtn_Click(null,null);
            }
        }

        private void SendBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            _p.StandardInput.WriteLine(Contents.Text);
            Contents.Text = string.Empty;
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
            process.StartInfo.StandardInputEncoding = System.Text.Encoding.UTF8;
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
                    Scrl.ScrollToEnd();
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
                    if(e.Data.Contains("Starting Minecraft server on *:"))
                    {
                        _p.StandardInput.WriteLine();
                        var inp = e.Data.IndexOf("*:");
                        PortTB.Text = e.Data.Substring(inp + 2);
                        var grid = PortTB.Parent as Grid;
                        grid.Background = new SolidColorBrush(Colors.Aqua);
                    }
                    Scrl.ScrollToEnd();
                });
            }
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ConsoleOutput.Text += "已退出，将会在 10 秒后关闭页面";
            });
            new Thread(() =>
            {
                Thread.Sleep(10000);
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Exited?.Invoke(this, e);
                });
            }).Start();
        }

        public void LauncherQuited()
        {
            _p.StandardInput.WriteLine("stop");
            new Thread(() =>
            {
                Thread.Sleep(5000);
                if (!_p.HasExited)
                {
                    _p.Kill();
                }
            }).Start();
            _p.WaitForExit();
        }

        public event EventHandler Exited;
    }
}
