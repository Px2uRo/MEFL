using MEFL.APIData;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using MEFL.Configs;
using Newtonsoft.Json;
#if WPF
using System.Windows.Controls;
using System.Windows.Data;
using MEFL.Controls;
#elif AVALONIA
using Avalonia.Data.Converters;
using Avalonia.Controls;
#endif

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
#if WPF
    public class HostingsToUI : IValueConverter
    {
        internal static StackPanel res = new StackPanel();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            res.Children.Clear();
            foreach (var item in (value as ObservableCollection<Hosting>))
            {
                var element = new MyExtensionCard() { Margin=new System.Windows.Thickness(0,0,0,15)};
                element.DataContext = new HostingModelView(item);
    #region 加载好之后才能做的事情
                //todo 这里能做一些等插件加载好之后才能做的事情
                var reg = JsonConvert.DeserializeObject<DownloaderConfig>(RegManager.Read("Downloader"));
                if (reg != null)
                {
                    foreach (var down in APIModel.Downloaders)
                    {
                        if (down.FileName == reg.FileName && down.Name == reg.DownloaderName)
                        {
                            APIModel.SelectedDownloader = down;
                        }
                    }
                }

                //var reg = RegManager.Read("Sources")
                foreach (var sources in APIModel.DownloadSources)
                {
                    if (sources.Value.Selected == null)
                    {
                        sources.Value.Selected = sources.Value[0];
                    }
                }
                Contract.Advanced.SetSelectedSources(APIModel.DownloadSources.Selected);
    #endregion
                res.Children.Add(element);
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
#endif
}
