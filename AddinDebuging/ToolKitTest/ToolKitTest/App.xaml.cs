namespace ToolKitTest;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		App.Current.UserAppTheme = AppTheme.Light;
		if (DeviceInfo.Idiom == DeviceIdiom.Phone)
		{
            MainPage = new AppShell();
        }
		else if (DeviceInfo.Idiom==DeviceIdiom.Tablet)
		{

		}
    }
}
