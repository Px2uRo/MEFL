using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MEFL.APIData;
using MEFL.Contract;
using MEFL.Controls;
using Newtonsoft.Json;

namespace MEFL.PageModelViews
{
    public class RealMainPageModelView:PageModelViewBase
    {
        public Contract.GameInfoBase CurretGame
        {
            get 
            {
                 return APIModel.CurretGame;
            }
            set 
            {
                APIModel.CurretGame = value;
                Invoke("CurretGame");
            }
        }

        public ObservableCollection<Contract.GameInfoBase> GameInfoConfigs
        {
            get
            {
                return APIModel.GameInfoConfigs;
            }
            set
            {
                APIModel.GameInfoConfigs = value;
                Invoke("GameInfoConfigs");
            }
        }

        public ObservableCollection<MEFLFolderInfo> MyFolders 
        {
            get
            {
                ObservableCollection<MEFLFolderInfo> res = new ObservableCollection<MEFLFolderInfo>() { { new MEFLFolderInfo(System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"), "本地文件夹") } };
                foreach (var item in APIModel.MyFolders)
                {
                    res.Add(item);
                }
                return res;
            }
            set
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i].Path == System.IO.Path.Combine(Environment.CurrentDirectory, ".minecraft"))
                    {
                        value.RemoveAt(i);
                    }
                }
                APIModel.MyFolders = value;
                Invoke("MyFolders");
            }
        }

        public int SelectedFolderIndex
        {
            get
            {
                return APIData.APIModel.SelectedFolderIndex; }
            set {
                if (value < 0)
                {
                    value = 0;
                }
                APIData.APIModel.SelectedFolderIndex = value;
                APIData.APIModel.GameInfoConfigs =MyFolders[value].Games;
                Invoke("GameInfoConfigs");
                Invoke("SelectedFolderIndex");
            }
        }

        public ICommand LuanchGameCommand
        {
            get { return RealMainPageModel.LuanchGameCommand; }
            set { RealMainPageModel.LuanchGameCommand = value; }
        }
        public ICommand AddFolderInfoCommand { get=> RealMainPageModel.AddFolderInfoCommand; set { RealMainPageModel.AddFolderInfoCommand = value; } }
    }

    public static class RealMainPageModel
    {
        public static ICommand LuanchGameCommand { get; set; }
        public static ICommand AddFolderInfoCommand { get; set; }
        static RealMainPageModel()
        {
            LuanchGameCommand = new LuanchGameCommand();
            AddFolderInfoCommand = new AddFolderInfoCommand();
        }
    }


    public class LuanchGameCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (APIModel.CurretGame != null)
            {
                if (APIModel.CurretGame.LaunchByLauncher == false)
                {
                    APIModel.CurretGame.Launch(APIModel.SettingArgs).Start();
                }
                else
                {
                    Process p = new Process();
                    
                }
            }
        }
    }

    public class AddFolderInfoCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            //Todo 注册表嘛！i18N嘛！
            string RegKey = "根目录";

            for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
            {
                if (((App.Current.Resources["MainPage"] as Grid).Children[i] as MyPageBase).Tag == "PickUP"|| ((App.Current.Resources["MainPage"] as Grid).Children[i] as MyPageBase).Tag == "RenameFolder"
                )
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new SpecialPages.PickUpAFolder() { Tag="PickUP",Visibility=Visibility.Hidden,Currect=RegKey});
            MyPageBase From = new MyPageBase();
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("PickUP", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }

            RegKey = null;
        }
    }
    public class ConvertGameInfo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (parameter.ToString() == "Name")
                {
                    try
                    {
                        return (value as GameInfoBase).Name;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
                else
                {
                    return "未知";
                }
            }
            else
            {
                return "未设置";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IndexToUI : IValueConverter
    {
        #region 一堆字段而已
        Thickness VarMargin = new Thickness(0, 0, 0, 2);
        Thickness VarBorderThickness = new Thickness(5);
        Brush VarBorderBrush = App.Current.Resources["SYTLE_Standard_BorderBrush"] as SolidColorBrush;
        CornerRadius VarConrnerRadius = new CornerRadius(5);
        StackPanel MyGamesSP = new StackPanel();
        #endregion

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MyGamesSP.Children.Clear();

            #region 收藏夹而已
            List<Controls.MyItemsCard> cards = new List<Controls.MyItemsCard>();
            Controls.MyItemsCard favorcard = new Controls.MyItemsCard() { 
                IsAbleToSwap = true, Title = "Favorite", 
                Margin = VarMargin, BorderThickness = VarBorderThickness, 
                BorderBrush = VarBorderBrush, CornerRadius = VarConrnerRadius };
            ObservableCollection<Contract.GameInfoBase> favoritem = new ObservableCollection<Contract.GameInfoBase>();
            foreach (var item in APIModel.GameInfoConfigs)
            {
                if (item.IsFavorate)
                {
                    favoritem.Add(item);
                }
            }
            if (favoritem.Count > 0)
            {
                favorcard.ItemsSource = favoritem;
                cards.Add(favorcard);
            }
            #endregion

            #region 确定有多少卡片而已
            List<string> recorded = new List<string>();
            foreach (var item in APIModel.GameInfoConfigs)
            {
                if (recorded.Contains(item.ToString()) != true)
                {
                    cards.Add(new Controls.MyItemsCard() { IsAbleToSwap = true, Title = item.ToString(), Margin = VarMargin, BorderThickness = VarBorderThickness, BorderBrush = VarBorderBrush, CornerRadius = VarConrnerRadius });
                    recorded.Add(item.ToString());
                }
            }
            #endregion

            #region 给卡片加 ItemSources 而已
            for (int i = 0; i < cards.Count; i++)
            {
                var cardItemSources = new ObservableCollection<Contract.GameInfoBase>();
                foreach (var item in APIModel.GameInfoConfigs)
                {
                    if (item.ToString() == cards[i].Title.ToString())
                    {
                        cardItemSources.Add(item);
                    }
                }
                cards[i].ItemsSource = cardItemSources;
            }
            #endregion

            foreach (var item in cards)
            {
                MyGamesSP.Children.Add(item);
            }

            return MyGamesSP;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
