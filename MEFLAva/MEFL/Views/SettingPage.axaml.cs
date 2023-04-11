using Avalonia.Controls;
using MEFL.APIData;
using MEFL.PageModelViews;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MEFL.Views
{
    public partial class SettingPage : UserControl
    {
        internal static IControl UI = new SettingPage();
        ObservableCollection<string> itms = new ObservableCollection<string>();

        public SettingPage()
        {
            InitializeComponent();
            var dc = new SettingPageModelView();
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
        }

        private void RefreshJavas_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var smv = (SettingPageModelView)DataContext;
            smv.EnableSearchJava = false;
            APIModel.SearchJavas();
        }
    }
}
