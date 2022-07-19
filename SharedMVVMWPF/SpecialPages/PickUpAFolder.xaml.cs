using MEFL.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ModelView = MEFL.PageModelViews.PickUpAFolderPageModelView;

namespace MEFL.SpecialPages
{
    /// <summary>
    /// PickUpAFolder.xaml 的交互逻辑
    /// </summary>
    public partial class PickUpAFolder : MyPageBase
    {
        public PickUpAFolder()
        {
            InitializeComponent();
            (this.DataContext as ModelView).Curret= "根目录";
        }

        private void Changed(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Items")
            {
                PART_UniformGrid.Children.Clear();
                foreach (var item in (this.DataContext as ModelView).Items)
                {
                    PART_UniformGrid.Children.Add(new TextBlock() { Text=item.Name});
                }
            }
        }

        private void ErrorAppeard(object sender, ValidationErrorEventArgs e)
        {
            (sender as TextBox).Text = e.Error.ErrorContent as String;
        }

        private void Moving(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                (sender as TextBox).SelectAll();
            }
        }
    }
}
