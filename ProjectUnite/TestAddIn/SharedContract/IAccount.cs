using System.Collections.Generic;
using MEFL.Arguments;
#if WPF
using MEFL.Contract.Controls;
#elif AVALONIA
using AddAccountItem = Avalonia.Controls.Button;
#endif

namespace MEFL.Contract;

public interface IAccount
{
	AccountBase[] GetSingUpAccounts(SettingArgs args);

	AddAccountItem[] GetSingUpPage(SettingArgs args);
}
