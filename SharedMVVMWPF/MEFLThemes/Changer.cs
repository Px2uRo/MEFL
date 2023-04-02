using System.Windows.Media;

namespace MEFL.MEFLThemes
{
    public static class ThemeChanger
    {
        public static void Change(int index)
        {
            var ssc = Color.FromRgb(0,255,255);
            var sscb = new SolidColorBrush(ssc);
            var ssbg = new SolidColorBrush(Color.FromRgb(255,255,255));
            var ssbb = new SolidColorBrush(ssc);
            var ssfg = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            var scfg = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            if (index == 0)
            {
                App.Current.Resources["SYTLE_Standard_Color"] = ssc;
                App.Current.Resources["SYTLE_Standard_ColorBrush"] = sscb;
                App.Current.Resources["SYTLE_Standard_Background"] = ssbg;
                App.Current.Resources["SYTLE_Standard_BorderBrush"] = ssbb;
                App.Current.Resources["SYTLE_Standard_Foreground"] = ssfg;
                App.Current.Resources["SYTLE_Changing_Foreground"] = scfg;
            }
            else if (index == 1)
            {
                 ssc = Color.FromRgb(75, 255, 131);
                 sscb = new SolidColorBrush(ssc);
                 ssbg = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                 ssbb = new SolidColorBrush(ssc);
                 ssfg = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                 scfg = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                App.Current.Resources["SYTLE_Standard_Color"] = ssc;
                App.Current.Resources["SYTLE_Standard_ColorBrush"] = sscb;
                App.Current.Resources["SYTLE_Standard_Background"] = ssbg;
                App.Current.Resources["SYTLE_Standard_BorderBrush"] = ssbb;
                App.Current.Resources["SYTLE_Standard_Foreground"] = ssfg;
                App.Current.Resources["SYTLE_Changing_Foreground"] = scfg;
            }
            else if(index == 2)
            {
                ssc = Color.FromRgb(255, 255, 125);
                sscb = new SolidColorBrush(ssc);
                ssbg = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ssbb = new SolidColorBrush(ssc);
                ssfg = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                scfg = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                App.Current.Resources["SYTLE_Standard_Color"] = ssc;
                App.Current.Resources["SYTLE_Standard_ColorBrush"] = sscb;
                App.Current.Resources["SYTLE_Standard_Background"] = ssbg;
                App.Current.Resources["SYTLE_Standard_BorderBrush"] = ssbb;
                App.Current.Resources["SYTLE_Standard_Foreground"] = ssfg;
                App.Current.Resources["SYTLE_Changing_Foreground"] = scfg;
            }
            else if(index == 3)
            {
                ssc = Color.FromRgb(255, 216, 93);
                sscb = new SolidColorBrush(ssc);
                ssbg = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ssbb = new SolidColorBrush(ssc);
                ssfg = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                scfg = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                App.Current.Resources["SYTLE_Standard_Color"] = ssc;
                App.Current.Resources["SYTLE_Standard_ColorBrush"] = sscb;
                App.Current.Resources["SYTLE_Standard_Background"] = ssbg;
                App.Current.Resources["SYTLE_Standard_BorderBrush"] = ssbb;
                App.Current.Resources["SYTLE_Standard_Foreground"] = ssfg;
                App.Current.Resources["SYTLE_Changing_Foreground"] = scfg;
            }
            else if(index == 4)
            {
                ssc = Color.FromRgb(255, 175, 177);
                sscb = new SolidColorBrush(ssc);
                ssbg = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ssbb = new SolidColorBrush(ssc);
                ssfg = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                scfg = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                App.Current.Resources["SYTLE_Standard_Color"] = ssc;
                App.Current.Resources["SYTLE_Standard_ColorBrush"] = sscb;
                App.Current.Resources["SYTLE_Standard_Background"] = ssbg;
                App.Current.Resources["SYTLE_Standard_BorderBrush"] = ssbb;
                App.Current.Resources["SYTLE_Standard_Foreground"] = ssfg;
                App.Current.Resources["SYTLE_Changing_Foreground"] = scfg;
            }
        }
    }
}
