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
    public class GenerlManageGameModelView : PageModelViewBase
    {
        public GameInfoBase Account { get => GenerlGameManageModel.Game; set { GenerlGameManageModel.Game = value;Invoke("Account"); } }
        public ICommand ManageGameKillCommand { get; set; }
        public GenerlManageGameModelView(GameInfoBase game)
        {
            Account = game;
            ManageGameKillCommand = new ManageGameKillCommand();
        }
    }

    public class ManageGameKillCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            //todo 给爷重写
        }
    }

    public static class GenerlGameManageModel
    {
        public static GameInfoBase Game { get; set; }
        public static GenerlAddAccountModelView ModelView { get; set; }
    }
}
