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

namespace MEFL.SpecialPages
{
    /// <summary>
    /// MEFLRealseTypeManage.xaml 的交互逻辑
    /// </summary>
    public partial class MEFLRealseTypeManage : UserControl
    {
        public Process P { get; set; }
        public MEFLRealseTypeManage(Process Pro)
        {
            InitializeComponent();
            ContentTB.Text = "你也许启动了，但是就是还在加载窗口";
            P = Pro;
            P.Exited += P_Exited;
        }

        private void P_Exited(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ContentTB.Text = "退出了，或者启动失败了";
            });
        }
    }
}
