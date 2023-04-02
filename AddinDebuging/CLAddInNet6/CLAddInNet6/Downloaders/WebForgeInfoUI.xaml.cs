using CoreLaunching.Forge;
using MEFL.CLAddIn.GameTypes;
using MEFL.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MEFL.CLAddIn.Downloaders
{
    /// <summary>
    /// WebForgeInfoUI.xaml 的交互逻辑
    /// </summary>
    public partial class WebForgeInfoUI : UserControl
    {
        private DoubleAnimation _dbAni = new DoubleAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(0.3))
        };

        public WebForgeInfo Info;
        public WebForgeInfoUI(WebForgeInfo info)
        {
            InitializeComponent();
            Info= info;
            VersionBlock.Text = info.Version.ToString();
            CLGameType.ForgeStream.Seek(0, 0);
            ForgeImage.Source = new PngBitmapDecoder(CLGameType.ForgeStream, BitmapCreateOptions.None, BitmapCacheOption.Default).Frames[0];
            this.MouseEnter += WebForgeInfoUI_MouseEnter;
            MouseLeave += WebForgeInfoUI_MouseLeave;
        }

        private void WebForgeInfoUI_MouseLeave(object sender, MouseEventArgs e)
        {
            _dbAni.From = 0.5;
            _dbAni.To = 0;
            Rect.BeginAnimation(Rectangle.OpacityProperty,_dbAni);
        }

        private void WebForgeInfoUI_MouseEnter(object sender, MouseEventArgs e)
        {
            _dbAni.From = 0;
            _dbAni.To = 0.5;
            Rect.BeginAnimation(Rectangle.OpacityProperty, _dbAni);
        }
    }
}
