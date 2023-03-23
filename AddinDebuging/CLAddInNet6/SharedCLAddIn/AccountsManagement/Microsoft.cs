using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MEFL.CLAddIn.AccountsManagement
{
    internal class MicrosoftList: ObservableCollection<CLAddIn.MEFLMicrosoftAccount>
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
                try
                {
                    var tar = JsonConvert.DeserializeObject<MicrosoftList>(source);
                    if(tar == null)
                    {
                        return new();
                    }
                    else
                    {
                        return tar;
                    }
                }
                catch (Exception ex)
                {
                    return new();
                }
            }
        }

        public void AddOne(MEFLMicrosoftAccount account)
        {
            Model.MicrosoftAccounts.Add(account);
            RegManager.Write("MicrosoftAccounts", JsonConvert.SerializeObject(Model.MicrosoftAccounts));
        }
    }
}