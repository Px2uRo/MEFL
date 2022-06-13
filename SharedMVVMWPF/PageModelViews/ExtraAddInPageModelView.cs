using MEFL.ControlModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MEFL.PageModelViews
{
    public class ExtraAddInPageModelView:ControlModelViews.PageModelView
    {
        public ExtraAddInPageModelView()
        {

        }

        public void Ini(object? sender, EventArgs e)
        {
            foreach (var item in (App.Current.Resources["EPMV"] as ExtensionPageModelView).Hostings)
            {
                Border bd = new Border();
                bd.Width = 45;
                bd.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                (sender as StackPanel).Children.Add(bd);
            }
        }
    }
}
