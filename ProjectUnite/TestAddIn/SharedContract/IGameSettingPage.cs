using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MEFL.Contract
{
    public interface IGameSettingPage:IDialogContent
    {
        event EventHandler<GameInfoBase> OnSelected;
        event EventHandler<GameInfoBase> OnRemoved;
        event EventHandler<GameInfoBase> OnPageBack;
        event EventHandler<GameInfoBase> OnListUpdate;
    }
}
