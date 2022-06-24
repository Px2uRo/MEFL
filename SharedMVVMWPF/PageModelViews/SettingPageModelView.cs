using MEFL.ControlModelViews;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MEFL.PageModelViews
{
    public class SettingPageModelView: MEFL.ControlModelViews.PageModelView
    {
        public ICommand ChangeBackground { get; set; }
        public int LangIndex {
            get
            {
                return SettingPageModel.LangIndex;
            }
            set 
            { 
                SettingPageModel.LangIndex = value;
                SettingPageModel.SetLang();
            } 
        }
        public SettingPageModelView()
        {
            LangIndex = SettingPageModel.LangIndex;
            ChangeBackground = new ChangeBackground();
        }
    }
    public static class SettingPageModel
    {
        public static Image img { get; set; }
        public static int LangIndex { get; set; }
        public static void SetLang()
        {
            ResourceDictionary dic = new ResourceDictionary();
            if (SettingPageModel.LangIndex == (int)LangID.zh_CN)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_CN.xaml");
            }
            else if (SettingPageModel.LangIndex == (int)LangID.zh_yue_CN)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_yue_CN.xaml");
            }
            else if (SettingPageModel.LangIndex == (int)LangID.zh_yue_HK)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_yue_HK.xaml");
            }
            else if (SettingPageModel.LangIndex == (int)LangID.zh_HK)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_HK.xaml");
            }
            else if (SettingPageModel.LangIndex == (int)LangID.zh_MO)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_MO.xaml");
            }
            else if (SettingPageModel.LangIndex == (int)LangID.zh_TW)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_TW.xaml");
            }
            else if (SettingPageModel.LangIndex == (int)LangID.zh_SG)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_SG.xaml");
            }
            else if (SettingPageModel.LangIndex == (int)LangID.en_US)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/en_US.xaml");
            }
            else if (SettingPageModel.LangIndex == (int)LangID.en_UK)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/en_UK.xaml");
            }
            foreach (DictionaryEntry item in dic)
            {
                App.Current.Resources[item.Key] = item.Value;
            }
            dic = null;
        }
        static SettingPageModel()
        {
            #region Langs
            if (CultureInfo.CurrentCulture.Name == "zh-CN")
            {
                LangIndex = 0;
            }
            else if (CultureInfo.CurrentCulture.Name == "zh-HK")
            {
                LangIndex = 3;
            }
            else if (CultureInfo.CurrentCulture.Name == "zh-MO")
            {
                LangIndex = 4;
            }
            else if (CultureInfo.CurrentCulture.Name == "zh-TW")
            {
                LangIndex = 5;
            }
            else if (CultureInfo.CurrentCulture.Name == "zh-SG")
            {
                LangIndex = 6;
            }
            else if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                LangIndex = 7;
            }
            else if (CultureInfo.CurrentCulture.Name == "en-UK")
            {
                LangIndex = 8;
            }
            else
            {
                LangIndex = 7;
            }
            SetLang();
            #endregion
            img = new Image();
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
        }
    }

    public enum LangID
    {
        zh_CN,
        zh_yue_CN,
        zh_yue_HK,
        zh_HK,
        zh_MO,
        zh_TW,
        zh_SG,
        en_US,
        en_UK,
        ja
    }

    public class ChangeBackground : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Title = App.Current.Resources["I18N_String_Setting_Custom_BackgroundImage_Open"] as String;
            o.Filter = "(*.jpg)|*.jpg|(*.png)|*.png";
            o.ShowDialog();
            (App.Current.Resources["Background"] as Grid).Children.Clear();
            try
            {
                SettingPageModel.img.Source = new BitmapImage(new Uri(o.FileName));
                (App.Current.Resources["Background"] as Grid).Children.Add(SettingPageModel.img);
                APIData.APIModel.SettingConfig.PicturePath=o.FileName;
                APIData.APIModel.SettingConfig.Update();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
