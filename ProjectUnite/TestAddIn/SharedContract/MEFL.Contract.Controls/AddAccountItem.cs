#if WPF
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MEFL.Contract.Controls;

public class AddAccountItem : UserControl
{
	private DoubleAnimation _dbAni = new DoubleAnimation
	{
		Duration = new Duration(TimeSpan.FromSeconds(0.3))
	};

	public static readonly DependencyProperty AddAccountContentProperty;

	public IAddAccountPage AddAccountContent
	{
		get
		{
			return (IAddAccountPage)GetValue(AddAccountContentProperty);
		}
		set
		{
			SetValue(AddAccountContentProperty, value);
		}
	}

	private void BackgroundRect_MouseLeave(object sender, MouseEventArgs e)
	{
		_dbAni.From = 0.2;
		_dbAni.To = 0.0;
		(base.Template.FindName("PART_Rect", this) as Rectangle).BeginAnimation(UIElement.OpacityProperty, _dbAni);
	}

	private void BackgroundRect_MouseEnter(object sender, MouseEventArgs e)
	{
		_dbAni.From = 0.0;
		_dbAni.To = 0.2;
		(base.Template.FindName("PART_Rect", this) as Rectangle).BeginAnimation(UIElement.OpacityProperty, _dbAni);
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		base.MouseEnter += BackgroundRect_MouseEnter;
		base.MouseLeave += BackgroundRect_MouseLeave;
	}

	static AddAccountItem()
	{
		AddAccountContentProperty = DependencyProperty.Register("AddAccountContent", typeof(FrameworkElement), typeof(AddAccountItem), new PropertyMetadata(null));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AddAccountItem), new FrameworkPropertyMetadata(typeof(AddAccountItem)));
	}
}
#endif
