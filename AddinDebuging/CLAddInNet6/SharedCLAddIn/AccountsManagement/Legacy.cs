using MEFL.CLAddIn.Export;
using MEFL.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
#if WPF
using MEFL.CLAddIn.Pages;
#elif AVALONIA
using CLAddIn.Views;
#endif

namespace MEFL.CLAddIn.AccountsManagement
{
    internal class LegacyList : ObservableCollection<CLAddIn.MEFLLegacyAccount>
    {
        public void WriteReg()
        {
            RegManager.Write("LegacyAccounts", JsonConvert.SerializeObject(this));
        }
        public static LegacyList GetReg() {
            LegacyList res = new();
            try
            {
                var reg = RegManager.Read("LegacyAccounts");
                if (!string.IsNullOrEmpty(reg))
                {
                    var jOb = JToken.Parse(reg);
                    foreach (var item in jOb)
                    {
                        if (Guid.TryParse(item["Uuid"].ToString(), out var uuid))
                        {
                            res.Add(new(item["UserName"].ToString(), uuid));
                        }
                    }
                }
                else
                {
                    RegManager.Write("LegacyAccounts", "[]");
                }
            }
            catch (Exception ex)
            {
                res = new();
                //todo HandleException
            }
            return res;
        }

        public static void AddOne(MEFLLegacyAccount account)
        {
            Model.LegacyAccounts.Add(account);
            RegManager.Write("LegacyAccounts", JsonConvert.SerializeObject(Model.LegacyAccounts));
        }

        internal static void RemoveOne(MEFLLegacyAccount account)
        {
            Model.LegacyAccounts.Remove(account);
            RegManager.Write("LegacyAccounts", JsonConvert.SerializeObject(Model.LegacyAccounts));
        }
    }
}
