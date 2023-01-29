using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CoreLaunching;

namespace MEFL.CLAddIn.Pages
{
    /// <summary>
    /// MEFLRealseTypeManage.xaml 的交互逻辑
    /// </summary>
    public partial class MEFLRealseTypeManage : UserControl
    {
        private GameInfoBase game;
        public Process P { get; set; }
        public MEFLRealseTypeManage(Process Pro, GameInfoBase game)
        {
            InitializeComponent();
            P = Pro;
            this.game = game;
            ThisCard.Title = game.ToString();
            ContentTB.Text = $"[{DateTime.Now}] 等待窗口加载";
            P.Exited += P_Exited;
            P.OutputDataReceived += P_OutputDataReceived;
            P.BeginOutputReadLine();
        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ContentTB.Text += $"\n{e.Data}";
            });
        }

        private void P_Exited(object? sender, EventArgs e)
        {
            
            _t.Stop();
            var lnk = sender as Process;
            var msg = lnk.StandardError.ReadToEnd();
            if (lnk.ExitCode== 0)
            {
                Dispatcher.Invoke(() =>
                {
                    ContentTB.Text += $"\n[{DateTime.Now}]游戏正常退出";
                });
                return;
            }
            if (!String.IsNullOrEmpty(msg))
            {
                var help = CrashParser.Parse(msg, "zh_CN");
                Dispatcher.Invoke(() =>
                {
                    ContentTB.Text = $"\n[{DateTime.Now}]\n游戏崩溃了\n{msg}\n分析：\n{help}";
                });
            }
            else
            {
                msg = lnk.StandardOutput.ReadToEnd();
                var help = CrashParser.Parse(msg, "zh_CN");
                Dispatcher.Invoke(() =>
                {
                    ContentTB.Text = $"\n[{DateTime.Now}]\n游戏崩溃了\n{msg}\n分析：\n{help}";
                });
            }
        }
    }
}
