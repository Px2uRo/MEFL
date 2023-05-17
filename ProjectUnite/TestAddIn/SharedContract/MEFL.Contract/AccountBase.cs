using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using MEFL.Arguments;
using Newtonsoft.Json;

#if AVALONIA
using FrameworkElement = Avalonia.Controls.Control;
#endif

namespace MEFL.Contract;

public abstract class AccountBase : MEFLClass,INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
#if AVALONIA

    public virtual void PropChange([CallerMemberName] string Prop = "")
#elif WPF

	public virtual void PropChange(string Prop)
#endif
    {
		if (PropertyChanged != null)
		{
			PropertyChanged.Invoke(this,new(Prop));
		}
	}

    public override string ToString()
	{
		return $"{UserName} {Uuid}";
	}
	[JsonIgnore]
	public virtual bool Selected { get; set; }

    public abstract FrameworkElement ProfileAvator { get; }

	public abstract string UserName { get; set; }

	public abstract Guid Uuid { get; set; }

	public abstract string AccessToken { get; set; }

	public abstract string ClientID { get; set; }

	public abstract string Xuid { get; set; }

	public abstract string UserType { get; set; }

	public abstract string UserProperties { get; set; }

#if WPF
	public abstract object WelcomeWords { get; }
#endif
    public abstract IManageAccountPage ManagePage { get; }

	public abstract void LaunchGameAction(SettingArgs args);
}
