using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.CLAddIn.AccountsManagement
{
    public static class Model
    {
        public static List<AccountBase> List;
        public static LegacyList LegacyAccounts;
        public static MicrosoftList MicrosoftAccounts;

        static Model()
        {
            List = new List<AccountBase>();
            LegacyAccounts = LegacyList.GetReg();
            MicrosoftAccounts = MicrosoftList.GetReg();
            List.AddRange(LegacyAccounts);
            List.AddRange(MicrosoftAccounts);
        }
    }
}
