using System.Collections.Generic;
using MEFL.Arguments;
using MEFL.Contract.Controls;

namespace MEFL.Contract;

public interface IAccount
{
	AccountBase[] GetSingUpAccounts(SettingArgs args);

	AddAccountItem[] GetSingUpPage(SettingArgs args);
}
