using System;
using System.Collections.Generic;
using System.IO;
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

namespace MEFL.Pages
{
    /// <summary>
    /// RealMainPage.xaml 的交互逻辑
    /// </summary>
    public partial class RealMainPage : MEFL.Controls.MyPageBase
    {
        public RealMainPage()
        {
            InitializeComponent();
        }

        private void Ini(object? sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream(MEFL.Properties.Resources.StartGame);

            DiscreteObjectKeyFrame Base_Frame=new DiscreteObjectKeyFrame();
            var sen = sender as Image;
            var anim = new ObjectAnimationUsingKeyFrames();
            ObjectKeyFrameCollection cl = new ObjectKeyFrameCollection();
            List<BitmapFrame> fl = new List<BitmapFrame>();
            GifBitmapDecoder decoder = new GifBitmapDecoder(
                              ms,
                              BitmapCreateOptions.PreservePixelFormat, 
                              BitmapCacheOption.OnLoad);
            fl.AddRange(decoder.Frames);
            foreach (var item in fl)
            {
                if(Base_Frame == null)
                {
                    Base_Frame = new DiscreteObjectKeyFrame(item);
                    cl.Add(Base_Frame);
                }
                else
                {
                    Base_Frame = new DiscreteObjectKeyFrame(item);
                    cl.Add(Base_Frame);
                }
            }
            anim.KeyFrames = cl;
            anim.Duration = TimeSpan.FromSeconds(2);
            anim.RepeatBehavior = RepeatBehavior.Forever;
            anim.SpeedRatio = 0.1;
            sen.BeginAnimation(Image.SourceProperty, anim);
        }
    }
}
