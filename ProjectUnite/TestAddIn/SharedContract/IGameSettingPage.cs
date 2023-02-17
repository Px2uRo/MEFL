using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MEFL.Contract
{
    public interface IGameSettingPage
    {
        event EventHandler<EventArgs> OnSelected;
        event EventHandler<EventArgs> OnRemoved;
        event EventHandler<EventArgs> OnPageBack;
        event EventHandler<EventArgs> OnListUpdate;
    }
}
