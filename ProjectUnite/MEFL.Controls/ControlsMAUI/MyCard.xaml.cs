using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;

namespace MEFL.Controls;

public partial class MyCard : ContentView
{
	public MyCard()
	{
        InitializeComponent();
        (this.Resources["RES_StrokeShape"] as RoundRectangle).CornerRadius = CornerRadius;
 
    }
    private void ChangeCornerRadius(CornerRadius value)
    {
        string xaml = "<ControlTemplate xmlns=\"http://schemas.microsoft.com/dotnet/2021/maui\" xmlns:x=\"http://schemas.microsoft.com/winfx/2009/xaml\">" +
    "<Border StrokeThickness=\"{TemplateBinding BorderThickness}\" Stroke=\"{TemplateBinding BorderBrush}\" x:Name=\"PART_Border\">" +
    "<Border.StrokeShape>" +
    $"<RoundRectangle CornerRadius=\"{value.TopLeft},{value.TopRight},{value.BottomRight},{value.BottomLeft}\" x:Name=\"PART_StrokeShape\"/>" +
    "</Border.StrokeShape>" +
    "</Border>" +
    "</ControlTemplate>";
        ControlTemplate = new ControlTemplate().LoadFromXaml(xaml);
        xaml = string.Empty;
    }
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
    }

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
        set { SetValue(CornerRadiusProperty, value); ChangeCornerRadius(value); }
    }

    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(MyCard), new CornerRadius(20));


    public Brush BorderBrush
    {
        get { return (Brush)GetValue(BorderBrushProperty); }
        set { SetValue(BorderBrushProperty, value); }
    }

    public static readonly BindableProperty BorderBrushProperty = BindableProperty.Create(nameof(BorderBrush), typeof(Brush), typeof(MyCard), new SolidColorBrush(Colors.Black));

}