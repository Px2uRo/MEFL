using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MEFL.Controls
{
    public class ChangePageContentButton : RadioButton
    {
        //  <Rectangle Width="5" Height="35"
        //   Fill="{TemplateBinding SideBarBrush}" 
        //   Name="SideRect"/>

        static ChangePageContentButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChangePageContentButton), new FrameworkPropertyMetadata(typeof(ChangePageContentButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.IsChecked!=true)
            {
                (Template.FindName("SideRect", this) as Rectangle)
                    .Opacity = 0;
            }
            this.Checked += ChangePageContentButton_Checked;
            this.Unchecked += ChangePageContentButton_Unchecked;
        }

        public void ChangePageContentButton_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        public void ChangePageContentButton_Checked(object sender, RoutedEventArgs e)
        {
            int index = -1;
            var Panel = ((sender as ChangePageContentButton).Parent as StackPanel).Children;
            for (int i = 0; i < Panel.Count; i++)
            {
                //(Panel[i] as ChangePageContentButton)
                if ((Template.FindName("SideRect", (Panel[i] as ChangePageContentButton)) as Rectangle).Opacity == 1)
                {
                    index = i;
                }
            }
            if(index != -1)
            {
                (Template.FindName("SideRect", (Panel[index] as ChangePageContentButton)) as Rectangle).Opacity =0;
            }
            (Template.FindName("SideRect", this) as Rectangle)
                .Opacity = 1;
            Panel = null;
        }

        public Brush SideBarBrush
        {
            get { return (Brush)GetValue(SideBarBrushProperty); }
            set { SetValue(SideBarBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SideBarBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SideBarBrushProperty =
            DependencyProperty.Register("SideBarBrush", typeof(Brush), typeof(ChangePageContentButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0,0,0))));


    }
}
