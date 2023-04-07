using System.Collections.Generic;
using Avalonia.Controls;
using MEFL.Arguments;
#if WPF
using MEFL.Contract.Controls;
#endif

namespace MEFL.Contract;

public interface IAccount
{
	AccountBase[] GetSingUpAccounts(SettingArgs args);

	AddAccountItem[] GetSingUpPage(SettingArgs args);
}

#if AVALONIA
public class AddAccountItem
{
	public IAddAccountContent Dialog { get; set; }
	public Control Content { get; set; }

	public AddAccountItem(Control content, IAddAccountContent dialog)
	{
		Content= content;
        Dialog = dialog;
    }
}
#endif
