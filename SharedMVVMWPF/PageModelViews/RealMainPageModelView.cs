using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MEFL.APIData;

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
            if (APIModel.CurretGame != null)
            {
                if (parameter.ToString() == "Name")
                {
                    return APIModel.CurretGame.Name;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
