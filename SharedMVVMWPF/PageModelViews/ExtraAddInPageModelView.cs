using MEFL.ControlModelViews;
using MEFL.Controls;
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

        public static void Reload()
        {
            foreach (var item in (App.Current.Resources["EPMV"] as ExtensionPageModelView).Hostings)
            {
                if (item.IsOpen == true)
                {
                    ChangePageButton button = new ChangePageButton() { Width = 45 };
                    (App.Current.Resources["AddInChangePageButtons"] as StackPanel).Children.Add(button);
                }
            }
        }
    }
}
