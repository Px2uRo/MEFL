using System;
using System.Collections;
using System.Globalization;
using System.Windows.Input;
using MEFL.Arguments;
using System.Reflection;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using MEFL.APIData;
using Avalonia.Controls.ApplicationLifetimes;
using MEFL.Views;
using Avalonia.Threading;
#if WPF
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using Microsoft.Win32;
#elif AVALONIA
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using DynamicData;
#endif

namespace MEFL.PageModelViews
{
    public class IsJavaNullConverter : IValueConverter
    {
        ObservableCollection<String> res;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                if ((value as ObservableCollection<FileInfo>).Count == 0)
                {
                    if (parameter.ToString() == "ItemsSource")
                    {
                        return new string[1] { "请添加一个Java,如果没有看到的话，点击旁边的刷新按钮" };
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (parameter.ToString() == "ItemsSource")
                    {
                        res = new ObservableCollection<string>();
                        foreach (var item in value as ObservableCollection<FileInfo>)
                        {
                            var resitem = string.Empty;
                            try
                            {
                                FileVersionInfo fi = FileVersionInfo.GetVersionInfo(item.FullName);
                                resitem = $"VER:{fi.ProductMajorPart}，{item.FullName}";
                                GC.SuppressFinalize(fi);
                                fi = null;
                            }
                            catch (Exception ex)
                            {
                                resitem = $"VER:未知，{item.FullName}";
                            }
                            res.Add(resitem);
                        }
                        return res;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (parameter.ToString() == "ItemsSource")
                {
                    return new string[1] { "正在刷新" };
                }
                else
                {
                    return false;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class SettingPageModelView: PageModelViewBase
    {
        public string MaxMemory
        {
            get
            {
                return APIModel.MaxMemory.ToString();
            }
            set
            {
                try
                {
                    APIData.APIModel.MaxMemory = Convert.ToInt32(value);
                }
                catch (Exception ex)
                {
                }
                Invoke(nameof(MaxMemory));
            }
        }
        public DownloadSourceCollection DownSources => APIModel.DownloadSources;
        public string TempFolderPath
        {
            get { return APIModel.SettingConfig.TempFolderPath; }
            set { APIModel.SettingConfig.TempFolderPath = value;Invoke(nameof(TempFolderPath)); }
        }

        public string OtherJVMArgs
        {
            get { return APIData.APIModel.SettingConfig.OtherJVMArgs; }
            set { APIData.APIModel.SettingConfig.OtherJVMArgs = value; Invoke(nameof(OtherJVMArgs)); }
        }

        public ICommand ChangeBackgroundCommand { get; set; }
        public int LangIndex {
            get
            {
                return (int)APIData.APIModel.SettingArgs.LangID;
            }
            set 
            {
                APIData.APIModel.SettingArgs.LangID = (LangID)value;
                SettingPageModel.SetLang();
            }
        }

        public int SelectedJavaIndex
        {
            get { 
                int res = 0;
                for(int i = 0; i < Javas.Count; i++)
                {
                    if (Javas[i].FullName.ToString() == APIData.APIModel.SettingArgs.SelectedJava.FullName.ToString())
                    {
                        res = i;
                    }
                }
                return res;
            }
            set {
                try
                {
                    APIData.APIModel.SettingArgs.SelectedJava = Javas[value];
                    RegManager.Write("SelectedJava", Javas[value].FullName);
                }
                catch (Exception ex)
                {

                }
                Invoke("SelectedJavaIndex"); 
            }
        }

        public ObservableCollection<FileInfo> Javas { get => APIData.APIModel.Javas; set { APIData.APIModel.Javas = value;Invoke("Javas"); } }
        private bool _EnableSearchJava;

        public bool EnableSearchJava
        {
            get { return _EnableSearchJava; }
            set { _EnableSearchJava = value;Invoke("EnableSearchJava"); }
        }


        public DownloaderCollection Downloaders
        {
            get { return APIModel.Downloaders; }
        }

#if WPF
        public string SelectedDownloaderString
        {
            get {
                if (APIData.APIModel.SelectedDownloader == null)
                {
                    return "未选中下载器";
                }
                else
                {
                    return APIModel.SelectedDownloader.Name;
                }
            }
        }
#endif
        public SettingPageModelView()
        {
            _EnableSearchJava = true;
            LangIndex = (int)APIData.APIModel.SettingArgs.LangID;
            ChangeBackgroundCommand = new ChangeBackground();
        }
    }
    public static class SettingPageModel
    {
        public static string ContractVersion { get; set; }
#if WPF
        public static Image img { get; set; }
#elif AVALONIA
        public static IImage img { get; set; }
#endif
#if WPF
        public static void SetLang()
        {
            ResourceDictionary dic = new ResourceDictionary();
            if (APIData.APIModel.SettingArgs.LangID == LangID.zh_CN)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_CN.xaml");
            }
            else if (APIData.APIModel.SettingArgs.LangID == LangID.zh_yue_CN)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_yue_CN.xaml");
            }
            else if (APIData.APIModel.SettingArgs.LangID == LangID.zh_yue_HK)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_yue_HK.xaml");
            }
            else if (APIData.APIModel.SettingArgs.LangID == LangID.zh_HK)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_HK.xaml");
            }
            else if (APIData.APIModel.SettingArgs.LangID == LangID.zh_MO)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_MO.xaml");
            }
            else if (APIData.APIModel.SettingArgs.LangID == LangID.zh_TW)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_TW.xaml");
            }
            else if (APIData.APIModel.SettingArgs.LangID == LangID.zh_SG)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_SG.xaml");
            }
            else if (APIData.APIModel.SettingArgs.LangID == LangID.en_US)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/en_US.xaml");
            }
            else if (APIData.APIModel.SettingArgs.LangID == LangID.en_UK)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/en_UK.xaml");
            }
            foreach (DictionaryEntry item in dic)
            {
                App.Current.Resources[item.Key] = item.Value;
            }
            dic = null;
        }
#elif AVALONIA
        public static void SetLang()
        {
            //TODO Avalonia 自己的搞法
        }
#endif
        internal static SettingPageModelView ModelView = new();
        static SettingPageModel()
        {
#region 获取 MEFL.Contract 协议版本
            Assembly ass;
            try
            {
                Assembly[] assblies = AppDomain.CurrentDomain.GetAssemblies();
                ass = assblies[0];
                foreach (var item in assblies)
                {
                    if (item.FullName.Contains("Contract"))
                    {
                        ass = item;
                        break;
                    }
                }
                assblies = null;
                if (ass.FullName.Contains("Contract"))
                {
                    foreach (var item in ass.CustomAttributes)
                    {
                        if (item.AttributeType==typeof(AssemblyFileVersionAttribute))
                        {
                            ContractVersion = item.ConstructorArguments[0].Value.ToString();
                            break;
                        }
                    }
                }
                else
                {
                    ContractVersion = "Unknown";
                }
            }
            catch (Exception ex)
            {
                ContractVersion = "Unknown";
            }
            ass = null;
#endregion
#region Langs
            if (CultureInfo.CurrentCulture.Name == "zh-CN")
            {
                APIData.APIModel.SettingArgs.LangID = LangID.zh_CN;
            }
            else if (CultureInfo.CurrentCulture.Name == "zh-HK")
            {
                APIData.APIModel.SettingArgs.LangID = LangID.zh_HK;
            }
            else if (CultureInfo.CurrentCulture.Name == "zh-MO")
            {
                APIData.APIModel.SettingArgs.LangID = LangID.zh_MO;
            }
            else if (CultureInfo.CurrentCulture.Name == "zh-TW")
            {
                APIData.APIModel.SettingArgs.LangID = LangID.zh_TW;
            }
            else if (CultureInfo.CurrentCulture.Name == "zh-SG")
            {
                APIData.APIModel.SettingArgs.LangID = LangID.zh_SG;
            }
            else if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                APIData.APIModel.SettingArgs.LangID = LangID.en_US;
            }
            else if (CultureInfo.CurrentCulture.Name == "en-UK")
            {
                APIData.APIModel.SettingArgs.LangID = LangID.en_UK;
            }
            else
            {
                APIData.APIModel.SettingArgs.LangID = LangID.en_US;
            }
            SetLang();
            #endregion
            #region 获取背景图片
#if WPF

            img = new Image();
            img.Stretch = Stretch.UniformToFill;
            if (APIData.APIModel.SettingConfig.PicturePath != null)
            {
                try
                {
                    img.Source = new BitmapImage(new Uri(APIData.APIModel.SettingConfig.PicturePath));
                }
                catch (Exception ex)
                {

                }
            }
#elif AVALONIA
            if (APIData.APIModel.SettingConfig.PicturePath != null)
            {
                if (App.Current.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime desktop)
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        if (desktop.MainWindow is MainWindow window)
                        {

                            var image = new Avalonia.Media.Imaging.Bitmap(APIData.APIModel.SettingConfig.PicturePath);
                            var imaCont = new Avalonia.Controls.Image() { Source = image };
                            imaCont.Stretch = Stretch.UniformToFill;
                            window.BackGround.Children.Clear();
                            window.BackGround.Children.Add(imaCont);
                        }
                    });
                }
            }
#endif
            #endregion
        }
    }
    public class ChangeBackground : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }
#if WPF
        public void Execute(object? parameter)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Title = App.Current.Resources["I18N_String_Setting_Custom_BackgroundImage_Open"] as String;
            o.Filter = "(*.jpg)|*.jpg|(*.png)|*.png";
            o.ShowDialog();
            try
            {
                if (o.FileName!=String.Empty)
                {
                    (App.Current.Resources["Background"] as Grid).Children.Clear();
                    SettingPageModel.img.Source = new BitmapImage(new Uri(o.FileName));
                    (App.Current.Resources["Background"] as Grid).Children.Add(SettingPageModel.img);
                    APIData.APIModel.SettingConfig.PicturePath = o.FileName;
                }
            }
            catch (Exception ex)
            {

            }
        }
#elif AVALONIA
        public async void Execute(object? parameter)
        {
            if(App.Current.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime desktop)
            {
                App.OpenFileDialog.AllowMultiple= false;
                App.OpenFileDialog.Filters.Clear();
                App.OpenFileDialog.Filters.Add(new() { Extensions = new(){"jpg","png"},Name="图片"});
                if(desktop.MainWindow is MainWindow window)
                {
                    var res = await App.OpenFileDialog.ShowAsync(window);
                    if(res!=null)
                    {
                        if (res.Length > 0)
                        {
                            var image = new Avalonia.Media.Imaging.Bitmap(res[0]);
                            var imaCont = new Avalonia.Controls.Image() { Source = image };
                            imaCont.Stretch = Stretch.UniformToFill;
                            window.BackGround.Children.Clear();
                            window.BackGround.Children.Add(imaCont);
                            APIData.APIModel.SettingConfig.PicturePath = res[0];
                        }
                    }
                }
            }
        }

#endif
    }
}
