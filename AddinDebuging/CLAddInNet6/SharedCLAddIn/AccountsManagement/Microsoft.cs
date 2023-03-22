using Newtonsoft.Json;
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
            var source = RegManager.Read("MicrosoftAccounts");
            if (string.IsNullOrEmpty(source))
            {
                RegManager.Write("MicrosoftAccounts", "[]");
                return new();
            }
            else
            {
                var res = new MicrosoftList();
                var arry = JArray.Parse(source);
                return res;
            }
        }

        public void AddOne(MEFLMicrosoftAccount account)
        {
            Model.MicrosoftAccounts.Add(account);
        }
    }
}