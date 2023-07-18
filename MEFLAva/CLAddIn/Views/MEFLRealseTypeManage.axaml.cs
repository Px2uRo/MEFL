using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using MEFL.CLAddIn;
using MEFL.Contract;
using System.Diagnostics;
using System.Text;
using Timer = System.Timers.Timer;

namespace CLAddIn.Views
{
    public partial class MEFLRealseTypeManage : UserControl,IProcessManagePage
    {
        Process _p;
        List<string> strings = new List<string>();
        Timer _t =new();
        FileSystemWatcher _sysw;
        public MEFLRealseTypeManage()
        {
            InitializeComponent();
        }

        public MEFLRealseTypeManage(Process process,GameInfoBase datacontext)
        {
            InitializeComponent();
            DataContext= datacontext;
            _p = process;

            process.StartInfo.RedirectStandardError = false;
            process.StartInfo.CreateNoWindow = false;
            Directory.CreateDirectory(Path.Combine(datacontext.GameFolder, "screenshots"));
            _sysw = new FileSystemWatcher();
            _sysw.Path = Path.Combine(datacontext.GameFolder, "screenshots");
            _sysw.NotifyFilter = NotifyFilters.FileName;
            //sysw.Filter = "*.png";
            _sysw.Created += Sysw_Created;
            _sysw.EnableRaisingEvents = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError= true;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_OutputDataReceived;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.Exited += Process_Exited;
            TB.Text = $"{DateTime.Now} 游戏启动，等待游戏加载";
            _t = new Timer(2000);
            _t.Elapsed += new((s, e) =>
            {
                var mem = ProcessUtil.GetMemof(process);
                Dispatcher.UIThread.InvokeAsync(new Action(() =>
                {
                    MemScan.Text = mem.ToString();
                }));
            });
            _t.Start();
            PreViewImage.PointerPressed += PreViewImage_PointerPressed;
        }

        private void PreViewImage_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            ImageViewerDialog.Show(PreViewImage.Tag as String, PreViewImage.Source as Bitmap);
        }

        private void Sysw_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                Thread.Sleep(1000);
                using (var fs = File.Open(e.FullPath, FileMode.Open))
                {
                    var img = new Bitmap(fs);
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        PreViewImage.Source = img;
                        PreViewImage.Tag = e.FullPath;
                    });
                }
            }
            catch (Exception)
            {

            }
        }

        private void ForceExit(object? sender, RoutedEventArgs e)
        {
            if (!_p.HasExited)
            {
                _p.Kill();
            }
            Exited.Invoke(this, e);
        }
        private void OpenSave(object? sender, RoutedEventArgs e)
        {
            if (DataContext is GameInfoBase g)
            {
                Process.Start("explorer.exe", Path.Combine(g.GameFolder, "saves"));
            }
        }
        private void OpenScreen(object? sender, RoutedEventArgs e)
        {
            if (DataContext is GameInfoBase g)
            {
                if(String.IsNullOrEmpty(PreViewImage.Tag as String))
                {
                    Process.Start("explorer.exe", Path.Combine(g.GameFolder, "screenshots"));
                }
                else
                {
                    Process.Start("explorer.exe", $"/select,\"{PreViewImage.Tag as String}\"");
                }
            }
        }
        private int _recorded = 0;
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (_recorded > 500)
            {
                return;
            }
            if (!String.IsNullOrWhiteSpace(e.Data))
            {
                _recorded++;
                if (_recorded > 500)
                {
                    new Thread(() => {
                        _p.EnableRaisingEvents= false;
                        Thread.Sleep(20000);
                        _recorded = 0;

                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            GC.SuppressFinalize(TB.Text);
                            TB.Text = string.Empty;
                            TB.Text = $"[{DateTime.Now}] 等待 Minecraft 响应";
                            GC.Collect();
                        });
                        _p.EnableRaisingEvents = true;
                    }).Start();
                    return;
                }
                if (strings.Count > 50) 
                {
                    if (strings[0] is not null)
                    {
                        GC.SuppressFinalize(strings[0]);
                        GC.Collect();
                        strings.RemoveAt(0);
                    }
                } 
                strings.Add(e.Data);
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    GC.SuppressFinalize(TB.Text);
                    TB.Text = string.Empty;
                    GC.Collect();
                    TB.Text = string.Join(Environment.NewLine, strings);
                    Scl.ScrollToEnd();
                });
            }
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            _t.Stop();
            _t.Close();
            if((sender as Process).ExitCode == 0)
            {
                Exited?.Invoke(this, e);
            }
            else
            {
                //Dispatcher.UIThread.InvokeAsync(() =>
                //{
                //    Scl.ScrollToEnd();
                //    TB.Text = TB.Text + "\n" + "异常退出";
                //    var btn = new Button() { HorizontalAlignment=Avalonia.Layout.HorizontalAlignment.Right,
                //        VerticalAlignment=Avalonia.Layout.VerticalAlignment.Center,Content="清除"};
                //    btn.Click += Btn_Click;
                //    //grid.Children.Add(btn);
                //});
            }
        }

        private void Btn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Exited?.Invoke(this, e);
        }

        public void LauncherQuited()
        {
            
        }

        public event EventHandler Exited;
    }
}
