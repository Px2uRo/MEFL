using Microsoft.VisualBasic.CompilerServices;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MEFL.Controls
{
    public class MyCard : UserControl
    {
        static MyCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyCard), new FrameworkPropertyMetadata(typeof(MyCard)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (TitleProperty == null)
            {

            }
            else if (Title is string)
            {
                TextBlock t = new TextBlock();
                t.Text = Title as string;
                (Template.FindName("PART_Title", this) as ContentPresenter).Content = t;
            }
            else
            {
                (Template.FindName("PART_Title", this) as ContentPresenter).Content = Title;
            }
        }

        public object Title
        {
            get { 

                return (object)GetValue(TitleProperty); 
            }
            set {
                SetValue(TitleProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(object), typeof(MyCard), new PropertyMetadata("Title"));
    }
}
