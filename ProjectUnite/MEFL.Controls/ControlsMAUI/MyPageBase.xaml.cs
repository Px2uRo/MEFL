namespace MEFL.Controls;

public partial class MyPageBase : ContentView
{
    #region consts
    public const string MyOpcityAni = "OpcityAni";
    public const string MyOpcityAniiBhv = "MyOpcityAniBhv";
    #endregion

    Animation HeightAni = null;

    public string Tag
    {
        get { return (string)GetValue(TagProperty); }
        set { SetValue(TagProperty, value); }
    }

    public static readonly BindableProperty TagProperty = BindableProperty.Create(nameof(Tag), typeof(string), typeof(MyPageBase), null);

    public MyPageBase()
	{
		InitializeComponent();
	}

    public void Show(MyPageBase From)
    {
        if (From.Tag as String != this.Tag as String)
        {
            From.Hide();
            Show();
        }
    }

    /*
             if (Statu)
        {
            Time = (uint)((Height / 0.5) * ControlModel.TimeMultiple);

            if (HeightAni != null)
            {
                HeightAni.Dispose();
            }
            HeightAni = new Animation(v => this.HeightRequest = v, Height, 40);
            HeightAni.Commit(this, MySwapAni,easing:Easing.CubicOut,length:Time);
        }
        else
        {
            Time = (uint)((ProbaHeight-Height) / 0.5 * ControlModel.TimeMultiple);

            if (HeightAni != null)
            {
                HeightAni.Dispose();
            }
            HeightAni = new Animation(v => this.HeightRequest = v, Height, ProbaHeight);
            HeightAni.Commit(this, MySwapAni, easing: Easing.CubicOut, length: Time);
        }
     */
    public void Show()
    {
        IsVisible = true;
        if (HeightAni != null)
        {
            HeightAni.Dispose();
        }
        HeightAni = new Animation(v => this.Opacity = v, 0, 1);
        HeightAni.Commit(this, MyOpcityAni, easing: Easing.CubicOut, length: 200);
    }

    public void Hide()
    {
        if (HeightAni != null)
        {
            HeightAni.Dispose();
        }
        HeightAni = new Animation(v => this.Opacity = v, 1, 0);
        HeightAni.Commit(this, MyOpcityAni, easing: Easing.CubicOut, length: 200);
        IsVisible = false;
    }
}