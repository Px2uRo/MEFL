using MEFL.Contract;
using MEFL.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MEFL.PageModelViews
{
    public class GenerlSettingGameModelView:PageModelViewBase
    {
        public GameInfoBase Game { get => GenerlSettingGameModel.Game; set { GenerlSettingGameModel.Game = value;Invoke("Game"); } }
        public ICommand SelectCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public static string CurrectName; 
        public static string CurrectUuid;
        public GenerlSettingGameModelView(GameInfoBase game)
        {
            Game = game;
            SelectCommand = new SelectGameCommand();
            BackCommand = new BackGameCommand();
            RemoveCommand = new RemoveGameCommand();
            GenerlSettingGameModel.ModelView = this;
        }
    }

    internal class SelectGameCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            //todo 重写
        }
    }

    internal class BackGameCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            //todo 重写
            MyPageBase From = new MyPageBase();
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("UserManagePage", (App.Current.Resources["MainPage"] as Grid)))
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
    }

    public class RemoveGameCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            //todo 重写
            GenerlSettingGameModel.Game.Dispose();
        }
    }

    public static class GenerlSettingGameModel
    {
        public static GameInfoBase Game { get; set; }
        public static GenerlSettingGameModelView ModelView { get; set; }
    }
}
