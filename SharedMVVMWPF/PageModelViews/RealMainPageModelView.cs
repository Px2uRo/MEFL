using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MEFL.APIData;
using MEFL.Contract;

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
                return APIModel.MyFolders;
            }
            set
            {
                APIModel.MyFolders = value;
                Invoke("MyFolders");
            }
        }

        public ICommand LuanchGameCommand
        {
            get { return RealMainPageModel.LuanchGameCommand; }
            set { RealMainPageModel.LuanchGameCommand = value; }
        }
        public RealMainPageModelView()
        {
            LuanchGameCommand = new LuanchGameCommand();
        }
    }

    public static class RealMainPageModel
    {
        public static ICommand LuanchGameCommand { get; set; }

        static RealMainPageModel()
        {

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
            MessageBox.Show("没准备好呐！");
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
}
