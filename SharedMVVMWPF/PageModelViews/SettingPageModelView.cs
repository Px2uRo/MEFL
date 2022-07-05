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
using MEFL.Arguments;
using System.Reflection;

namespace MEFL.PageModelViews
{
    public class SettingPageModelView: PageModelViewBase
    {
        public ICommand ChangeBackground { get; set; }
        public int LangIndex {
            get
            {
                return (int)SettingPageModel.SettingArgs.LangID;
            }
            set 
            {
                SettingPageModel.SettingArgs.LangID = (LangID)value;
                SettingPageModel.SetLang();
            }
        }
        public SettingPageModelView()
        {
            LangIndex = (int)SettingPageModel.SettingArgs.LangID;
            ChangeBackground = new ChangeBackground();
        }
    }
    public static class SettingPageModel
    {
        public static string ContractVersion { get; set; }
        public static Image img { get; set; }
        public static Arguments.SettingArgs SettingArgs { get; set; }
        public static void SetLang()
        {
            ResourceDictionary dic = new ResourceDictionary();
            if (SettingArgs.LangID == LangID.zh_CN)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_CN.xaml");
            }
            else if (SettingArgs.LangID == LangID.zh_yue_CN)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_yue_CN.xaml");
            }
            else if (SettingArgs.LangID == LangID.zh_yue_HK)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_yue_HK.xaml");
            }
            else if (SettingArgs.LangID == LangID.zh_HK)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_HK.xaml");
            }
            else if (SettingArgs.LangID == LangID.zh_MO)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_MO.xaml");
            }
            else if (SettingArgs.LangID == LangID.zh_TW)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_TW.xaml");
            }
            else if (SettingArgs.LangID == LangID.zh_SG)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/zh_SG.xaml");
            }
            else if (SettingArgs.LangID == LangID.en_US)
            {
                dic.Source = new Uri("pack://application:,,,/I18N/en_US.xaml");
            }
            else if (SettingArgs.LangID == LangID.en_UK)
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
            SettingArgs = new Arguments.SettingArgs();
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
                        if (item.AttributeType.ToString().Contains("AssemblyFileVersionAttribute"))
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
                SettingArgs.LangID = LangID.zh_CN;
            }
            else if (CultureInfo.CurrentCulture.Name == "zh-HK")
            {
                SettingArgs.LangID = LangID.zh_HK;
            }
            else if (CultureInfo.CurrentCulture.Name == "zh-MO")
            {
                SettingArgs.LangID = LangID.zh_MO;
            }
            else if (CultureInfo.CurrentCulture.Name == "zh-TW")
            {
                SettingArgs.LangID = LangID.zh_TW;
            }
            else if (CultureInfo.CurrentCulture.Name == "zh-SG")
            {
                SettingArgs.LangID = LangID.zh_SG;
            }
            else if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                SettingArgs.LangID = LangID.en_US;
            }
            else if (CultureInfo.CurrentCulture.Name == "en-UK")
            {
                SettingArgs.LangID = LangID.en_UK;
            }
            else
            {
                SettingArgs.LangID = LangID.en_US;
            }
            SetLang();
            #endregion
            #region 获取背景图片
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
                    APIData.APIModel.SettingConfig.Update();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
