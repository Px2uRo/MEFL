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

namespace MEFL.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MEFL.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MEFL.Controls;assembly=MEFL.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:ChangePageButton/>
    ///
    /// </summary>
    public class ChangePageButton : UserControl
    {
        private DoubleAnimation _dbani;

        static ChangePageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChangePageButton), new FrameworkPropertyMetadata(typeof(ChangePageButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _dbani = new DoubleAnimation();
            _dbani.EasingFunction = new PowerEase();
            this.MouseEnter += ChangePageButton_MouseEnter;
            this.MouseLeave += ChangePageButton_MouseLeave;
            this.MouseDown += ChangePageButton_MouseDown;
        }

        private void ChangePageButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MyPageBase From = new MyPageBase();
            bool Foo = false;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility==Visibility.Visible)
                {
                    From = item;
                    Foo = true;
                    if (item.Tag as String != From.Tag as String)
                    {
                        item.Hide();
                    }
                }
            }
            foreach (MyPageBase item in FindControl.FromTag(Tag, (App.Current.Resources["MainPage"] as Grid)))
            {
                if(Foo)
                {
                    item.Show(From);
                }
                else
                {
                    item.Show();
                }
            }
            From = null;
        }

        private void ChangePageButton_MouseLeave(object sender, MouseEventArgs e)
        {
            _dbani.From = 0.2;
            _dbani.To = 0;
            (Template.FindName("Icon", this) as Rectangle).BeginAnimation(OpacityProperty, _dbani);
        }

        private void ChangePageButton_MouseEnter(object sender, MouseEventArgs e)
        {
            _dbani.From = 0;
            _dbani.To = 0.2;
            (Template.FindName("Icon", this) as Rectangle).BeginAnimation(OpacityProperty, _dbani);
        }
    }
}
