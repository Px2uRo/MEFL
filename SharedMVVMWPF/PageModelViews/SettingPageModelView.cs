using MEFL.ControlModelViews;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MEFL.PageModelViews
{
    public class SettingPageModelView: MEFL.ControlModelViews.PageModelView
    {
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
        }
    }
    public static class SettingPageModel
    {
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
            else if(CultureInfo.CurrentCulture.Name == "en-UK")
            {
                LangIndex = 8;
            }
            else
            {
                LangIndex = 7;
            }
            SetLang();
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

}
