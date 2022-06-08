using MEFL.ControlModelViews;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MEFL.Pages
{
    /// <summary>
    /// ExtensionPage.xaml 的交互逻辑
    /// </summary>
    public partial class ExtensionPage : StackPanel
    {
        public ExtensionPage()
        {
            InitializeComponent();
            this.DataContext = MainModelViews.ExtensionPageModelView;
            foreach (var item in (this.DataContext as ExtensionPageModelView).Hostings)
            {
                this.Children.Add(new MyExtensionCard() { Hosting = item,Margin=new Thickness(0,0,0,15)});
            }
        }
        

    }
}
