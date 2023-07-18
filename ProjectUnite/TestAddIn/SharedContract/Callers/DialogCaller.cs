using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Callers
{
    public static class DialogCaller
    {
        public static event EventHandler<IDialogContent> ShowDialogEvent;

        public static void Show(IDialogContent dialog)
        {
            ShowDialogEvent?.Invoke(null, dialog);
        }
    }
}
