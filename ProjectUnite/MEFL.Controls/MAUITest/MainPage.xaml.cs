namespace MAUITest
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            var a = HelloCard.ControlTemplate.CreateContent();
        }
        
        private void OnCounterClicked(object sender, EventArgs e)
        {
            HelloCard.CornerRadius = new CornerRadius(new Random().Next(0,40));
        }

        private void Image_Focused(object sender, FocusEventArgs e)
        {
            throw new Exception();
        }
    }
}