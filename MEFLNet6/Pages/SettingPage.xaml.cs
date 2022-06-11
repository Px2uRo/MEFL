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

namespace MEFL.Pages
{
    /// <summary>
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPage : UserControl
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        private void VisibleChange(object sender, DependencyPropertyChangedEventArgs e)
        {
            DoubleAnimation ani;
            if (this.IsVisible)
            {
                ani = new DoubleAnimation { From = 0, To = 1, EasingFunction = new PowerEase(), Duration = TimeSpan.FromSeconds(1) };
                this.BeginAnimation(OpacityProperty, ani);
            }
            else
            {
                this.IsVisibleChanged -= VisibleChange;
                this.Visibility = Visibility.Visible;
                ani = new DoubleAnimation { From = 1, To = 0, EasingFunction = new PowerEase(), Duration = TimeSpan.FromSeconds(1) };
                this.BeginAnimation(OpacityProperty, ani);
                this.Visibility = Visibility.Hidden;
                this.IsVisibleChanged += VisibleChange;
            }
            ani = null;
        }

    }
}
