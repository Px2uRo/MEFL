using System.Collections.Generic;
using MEFL.Arguments;
using MEFL.Contract.Controls;

namespace MEFL.Contract;

public interface IAccount
{
	List<AccountBase> GetSingUpAccounts(SettingArgs args);

	List<AddAccountItem> GetSingUpPage(SettingArgs args);
}
