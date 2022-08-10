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
using MEFL.Controls;
using MEFL.PageModelViews;
using ModelView = MEFL.PageModelViews.ManageProcessesPageModelView;

namespace MEFL.SpecialPages
{
    /// <summary>
    /// ManageProcessesPage.xaml 的交互逻辑
    /// </summary>
    public partial class ManageProcessesPage : MyPageBase
    {
        public ManageProcessesPage()
        {
            InitializeComponent();
            this.DataContext = ManageProcessesPageModel.ModelView;
        }
    }
}
