using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MEFL.CLAddIn.AccountsManagement
{
    public class MicrosoftList: ObservableCollection<CLAddIn.MEFLMicrosoftAccount>
    {
        public static MicrosoftList GetReg()
        {
            return new();
        }
    }
}