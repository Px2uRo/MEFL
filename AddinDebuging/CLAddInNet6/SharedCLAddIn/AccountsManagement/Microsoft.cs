using MEFL.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MEFL.CLAddIn.AccountsManagement
{
    internal class MicrosoftList : ObservableCollection<CLAddIn.MEFLMicrosoftAccount>
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
                    if (tar == null)
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

        static internal void RemoveOne(MEFLMicrosoftAccount account)
        {
            Model.MicrosoftAccounts.Remove(account);
            RegManager.Write("MicrosoftAccounts", JsonConvert.SerializeObject(Model.MicrosoftAccounts));
        }
    }
    internal class UPList : ObservableCollection<CLAddIn.MEFLUnitedPassportAccount>
    {
        public static UPList GetReg()
        {
            var source = RegManager.Read("UnitedPassportAccounts");
            if (string.IsNullOrEmpty(source))
            {
                RegManager.Write("UnitedPassportAccounts", "[]");
                return new();
            }
            else
            {
                try
                {
                    var tar = JsonConvert.DeserializeObject<UPList>(source);
                    if (tar == null)
                    {
                        return new();
                    }
                    else
                    {
                        for (int i = 0; i < tar.Count; i++)
                        {
                            var item = tar[i];
                            item._cl.AccessToken = item.AccessToken;
                            item._cl.ClientToken = item.ClientToken;
                            item._cl.ServerID= item.ServerID;
                            item._cl.EmailAddress= item.EmailAddress;
                            item._cl.Nide8AuthPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CoreLaunching\\Nide8Auth.jar");
                        }
                        return tar;
                    }
                }
                catch (Exception ex)
                {
                    return new();
                }
            }
        }

        public void AddOne(MEFLUnitedPassportAccount account)
        {
            Model.UPList.Add(account);
            RegManager.Write("UnitedPassportAccounts", JsonConvert.SerializeObject(Model.UPList));
        }

        internal static void RemoveOne(MEFLUnitedPassportAccount account)
        {
            Model.UPList.Remove(account);
            RegManager.Write("UnitedPassportAccounts", JsonConvert.SerializeObject(Model.UPList));
        }
    }
}