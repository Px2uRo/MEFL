using MEFL.APIData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MEFL.Controls;

namespace MEFL.PageModelViews
{
    public class ExtensionPageModelView:PageModelViewBase
    {

        public ObservableCollection<Hosting> Hostings
        {
            get { return APIModel.Hostings; }
            set { Hostings = value; Invoke(nameof(Hostings)); }
        }
    }
    public class HostingsToUI : IValueConverter
    {
        static StackPanel res = new StackPanel();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            res.Children.Clear();
            foreach (var item in (value as ObservableCollection<Hosting>))
            {
                var element = new MyExtensionCard() { Margin=new System.Windows.Thickness(0,0,0,15)};
                element.DataContext = new HostingModelView(item);
                res.Children.Add(element);
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
