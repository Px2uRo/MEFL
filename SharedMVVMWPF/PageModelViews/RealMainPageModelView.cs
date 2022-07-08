using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using MEFL.APIData;

namespace MEFL.PageModelViews
{
    public class RealMainPageModelView:PageModelViewBase
    {
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
}
