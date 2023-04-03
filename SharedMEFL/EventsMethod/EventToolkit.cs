using MEFL.Contract;
#if WPF
using System.Windows.Controls;
using MEFL.Controls;
#elif AVALONIA

#endif
using MEFL.PageModelViews;
using MEFL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MEFL.EventsMethod
{
    internal static class EventToolkit
    {
        internal static void SettingsPage_OnListUpdate(object? sender, GameInfoBase e)
        {
            throw new NotImplementedException();
        }

#if WPF
        internal static void SettingsPage_OnSelected(object? sender, GameInfoBase e)
        {
            (App.Current.Resources["RMPMV"] as RealMainPageModelView).CurretGame = e;
            MyPageBase From = null;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("RealMainPage", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }
            for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
            {
                if ((App.Current.Resources["MainPage"] as Grid).Children[i] == From)
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
        }

        internal static void SettingsPage_OnRemoved(object? sender, GameInfoBase e)
        {
            throw new NotImplementedException();
        }

        internal static void SettingsPage_OnPageBack(object? sender, GameInfoBase e)
        {
            throw new NotImplementedException();
        }
#elif AVALONIA
        //TODO Avlonia 自己的搞法
#endif
    }
}
