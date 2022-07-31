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

namespace MEFL.Contract.Controls
{
    public class AddAccountItem : UserControl
    {
        private DoubleAnimation _dbAni = new DoubleAnimation() { Duration = new Duration(TimeSpan.FromSeconds(0.3)) };

        private void BackgroundRect_MouseLeave(object sender, MouseEventArgs e)
        {
            _dbAni.From = 0.2;
            _dbAni.To = 0.0;
            (Template.FindName("PART_Rect", this) as Rectangle).BeginAnimation(OpacityProperty, _dbAni);
        }

        private void BackgroundRect_MouseEnter(object sender, MouseEventArgs e)
        {
            _dbAni.From = 0.0;
            _dbAni.To = 0.2;
            (Template.FindName("PART_Rect", this) as Rectangle).BeginAnimation(OpacityProperty, _dbAni);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.MouseEnter += BackgroundRect_MouseEnter;
            this.MouseLeave += BackgroundRect_MouseLeave;
        }
        static AddAccountItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AddAccountItem), new FrameworkPropertyMetadata(typeof(AddAccountItem)));
        }



        public FrameworkElement AddAccountContent
        {
            get { return (FrameworkElement)GetValue(AddAccountContentProperty); }
            set { SetValue(AddAccountContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddAccountContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddAccountContentProperty =
            DependencyProperty.Register("AddAccountContent", typeof(FrameworkElement), typeof(AddAccountItem), new PropertyMetadata(null));



        public AccountBase FinnalReturn
        {
            get { return (AccountBase)GetValue(FinnalReturnProperty); }
            set { SetValue(FinnalReturnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FinnalReturn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FinnalReturnProperty =
            DependencyProperty.Register("FinnalReturn", typeof(AccountBase), typeof(AddAccountItem), new PropertyMetadata(null));


    }
}
