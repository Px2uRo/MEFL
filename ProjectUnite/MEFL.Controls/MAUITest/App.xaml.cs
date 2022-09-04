using Window=Microsoft.UI.Xaml.Window;

namespace MAUITest
{
    public partial class App : Application
    {
        public App()
        {
           InitializeComponent();

#if WINDOWS
            //new Window().Activate();
#endif
        }
    }
}