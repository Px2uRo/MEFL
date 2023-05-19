using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.CLAddIn.AccountsManagement
{
    internal static class Model
    {
        internal static List<AccountBase> List;
        internal static LegacyList LegacyAccounts;
        internal static MicrosoftList MicrosoftAccounts;
        internal static UPList UPList;

        static Model()
        {
            List = new List<AccountBase>();
            LegacyAccounts = LegacyList.GetReg();
            MicrosoftAccounts = MicrosoftList.GetReg();
            UPList = UPList.GetReg();
            List.AddRange(LegacyAccounts);
            List.AddRange(MicrosoftAccounts);
            List.AddRange(UPList);
        }
    }
}
