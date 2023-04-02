using System.Windows;
#if WINDOWS_UWP
using Microsoft.UI.Xaml;
#elif AVALONIA
using FrameworkElement = Avalonia.Controls.Control;
#endif

namespace MEFL.Contract;

public class IconTitlePagePair
{
	public string Title { get; private set; }

	public FrameworkElement Icon { get; private set; }

	public FrameworkElement Page { get; private set; }

	public IconTitlePagePair(string title, FrameworkElement icon, FrameworkElement page)
	{
		Title = title;
		Icon = icon;
		Page = page;
	}
}
