using Avalonia;
using Avalonia.Controls;
using MEFL.APIData;
using MEFL.Configs;
using MEFL.InfoControls;
using MEFL.PageModelViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MEFL.Views
{
    public partial class SettingPage : UserControl
    {
        internal static IControl UI = new SettingPage();
        ObservableCollection<string> itms = new ObservableCollection<string>();
        int downloadersCount = 0;
        DownloaderConfig downloaderConfig;

        protected override Size MeasureOverride(Size availableSize)
        {
            DownloadersGrid.Columns = (int)Math.Ceiling(availableSize.Width / 290.0);
            if (downloadersCount == 0)
            {
                DownloadersGrid.Rows = 1;
            }
            else
            {
                DownloadersGrid.Rows = (int)Math.Ceiling((double)downloadersCount / (double)DownloadersGrid.Columns);
            }
            return base.MeasureOverride(availableSize);
        }
        public SettingPage()
        {
            InitializeComponent();
            var dc = SettingPageModel.ModelView;
            this.DataContext = dc;
            dc.PropertyChanged += Dc_PropertyChanged;
            JavaList.SelectionChanged += JavaList_SelectionChanged;
            RefreshJavas.Click += RefreshJavas_Click;
            foreach (var item in dc.Javas)
            {
                itms.Add(item.FullName);
            }
            JavaList.Items =itms;
            JavaList.SelectedIndex = dc.SelectedJavaIndex;
            LoadDownloaderUI(dc.Downloaders);
        }

        private void LoadDownloaderUI(DownloaderCollection downloaders)
        {
            try
            {
                downloaderConfig = JsonConvert.DeserializeObject<DownloaderConfig>(RegManager.Read("Downloader"));
            }
            catch
            {
                downloaderConfig = null;
            }
            downloadersCount = downloaders.Count;
            DownloadersGrid.Children.Clear();
            foreach (var item in downloaders)
            {
                var btn = new SelecteDownloaderButton(item);
                DownloadersGrid.Children.Add(btn);
                if (downloaderConfig != null)
                {
                    if (downloaderConfig.DownloaderName == item.Name && item.FileName == downloaderConfig.FileName)
                    {
                        APIModel.SelectedDownloader = item;
                        btn.Enablebtn.IsChecked= true;
                    }
                }
            }
        }

        private void JavaList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var smv = (SettingPageModelView)DataContext;
            var inde = ((ComboBox)sender).SelectedIndex;
            if(inde!= -1)
            {
                APIModel.SettingArgs.SelectedJava = smv.Javas[inde];
                RegManager.Write("SelectedJava", smv.Javas[inde].FullName);
            }
        }

        private void Dc_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var dc = (SettingPageModelView)sender;
            if(e.PropertyName == "Javas")
            {
                var smv = (SettingPageModelView)sender;
                JavaList.SelectedIndex = -1;
                if (smv.EnableSearchJava)
                {
                    itms.Clear();
                    foreach (var item in smv.Javas)
                    {
                        itms.Add(item.FullName);
                    }
                }
            }
            else if (e.PropertyName == "Downloaders")
            {
                LoadDownloaderUI(dc.Downloaders);
            }
            else if(e.PropertyName==nameof(dc.DownSources))
            {

            }
        }

        private void RefreshJavas_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var smv = (SettingPageModelView)DataContext;
            smv.EnableSearchJava = false;
            APIModel.SearchJavas();
        }
    }
}
