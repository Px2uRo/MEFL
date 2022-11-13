using System.Windows;

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
