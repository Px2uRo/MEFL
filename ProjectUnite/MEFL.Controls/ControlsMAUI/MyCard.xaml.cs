using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;

namespace MEFL.Controls;

public partial class MyCard : ContentView
{
    #region Properties
    public bool IsSwaped
    {
        get { return (bool)GetValue(IsSwapedProperty); }
        set { SetValue(IsSwapedProperty, value);SwapChanged(value); }
    }

    public static readonly BindableProperty IsSwapedProperty = BindableProperty.Create(nameof(IsSwaped), typeof(bool), typeof(MyCard), false);


    public bool IsAbleToSwap
    {
        get { return (bool)GetValue(IsAbleToSwapProperty); }
        set { SetValue(IsAbleToSwapProperty, value); }
    }

    public static readonly BindableProperty IsAbleToSwapProperty = BindableProperty.Create(nameof(IsAbleToSwap), typeof(bool), typeof(MyCard), false);

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(MyCard), string.Empty);

    public double BorderThickness
    {
        get { return (double)GetValue(BorderThicknessProperty); }
        set { SetValue(BorderThicknessProperty, value); }
    }

    public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(double), typeof(MyCard), 5.0);

    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(MyCard), new CornerRadius(10));


    public Brush BorderBrush
    {
        get { return (Brush)GetValue(BorderBrushProperty); }
        set { SetValue(BorderBrushProperty, value); }
    }

    public static readonly BindableProperty BorderBrushProperty = BindableProperty.Create(nameof(BorderBrush), typeof(Brush), typeof(MyCard), new SolidColorBrush(Colors.Black));

    #endregion
    #region consts
    public const string MySwapAni = "SwapAni";
    public const string MySwapAniBhv = "MySwapAniBhv";
    #endregion
    Animation HeightAni = null;
    double ProbaHeight = 0.0;
    private uint Time;

    protected override Size ArrangeOverride(Rect bounds)
    {
        var res = base.ArrangeOverride(bounds);
        if (ProbaHeight == 0)
        {
            ProbaHeight = res.Height;
        }
        return res;
    }
    public MyCard()
	{
        InitializeComponent();
        this.Loaded += MyCard_Loaded;
        (this.Resources["RES_StrokeShape"] as RoundRectangle).CornerRadius = CornerRadius;
    }

    private void MyCard_Loaded(object sender, EventArgs e)
    {
        if (IsSwaped)
        {
            if (IsAbleToSwap)
            {
                HeightRequest = 40;
            }
            else
            {
                throw new Exception("«Îœ»‘ –Ì’€µ˛");
            }
        }
    }

    //private void ChangeCornerRadius(CornerRadius value)
    //{
    //    string xaml = "<ControlTemplate xmlns=\"http://schemas.microsoft.com/dotnet/2021/maui\" xmlns:x=\"http://schemas.microsoft.com/winfx/2009/xaml\">" +
    //"<Border StrokeThickness=\"{TemplateBinding BorderThickness}\" Stroke=\"{TemplateBinding BorderBrush}\" x:Name=\"PART_Border\">" +
    //"<Border.StrokeShape>" +
    //$"<RoundRectangle CornerRadius=\"{value.TopLeft},{value.TopRight},{value.BottomRight},{value.BottomLeft}\" x:Name=\"PART_StrokeShape\"/>" +
    //"</Border.StrokeShape>" +
    //"</Border>" +
    //"</ControlTemplate>";
    //    ControlTemplate = new ControlTemplate().LoadFromXaml(xaml);
    //    xaml = string.Empty;
    //}

    public void SwapChanged(bool Statu)
    {
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
    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }

    private void SwapMe(object sender, EventArgs e)
    {
        if (this.IsSwaped)
        {
            this.IsSwaped = false;
        }
        else
        {
            IsSwaped = true;
        }
    }
}