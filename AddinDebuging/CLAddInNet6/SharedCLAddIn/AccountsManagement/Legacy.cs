using MEFL.Contract;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MEFL.CLAddIn.AccountsManagement
{
    public class LegacyList : ObservableCollection<CLAddIn.MEFLLegacyAccount>
    {
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
    }
}
