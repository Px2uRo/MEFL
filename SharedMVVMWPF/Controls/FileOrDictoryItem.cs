using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MEFL.Controls
{
    [TemplatePart(Name = "Rect", Type = typeof(Rectangle))]
    public class FileOrDictoryItem: RadioButton
    {
        private DoubleAnimation _dbani = new DoubleAnimation();
        static FileOrDictoryItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileOrDictoryItem), new FrameworkPropertyMetadata(typeof(FileOrDictoryItem)));
        }

        public override void OnApplyTemplate()
        {
            this.MouseEnter += FileOrDictoryItem_MouseEnter;
            this.MouseLeave += FileOrDictoryItem_MouseLeave;
            base.OnApplyTemplate();
        }

        private void FileOrDictoryItem_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _dbani = new DoubleAnimation();
            _dbani.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            _dbani.To = 0.0;
            _dbani.From = 0.5;
            (Template.FindName("Rect", this) as Rectangle).BeginAnimation(OpacityProperty, _dbani);
        }

        private void FileOrDictoryItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _dbani = new DoubleAnimation();
            _dbani.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            _dbani.To = 0.5;
            _dbani.From = 0.0;
            (Template.FindName("Rect",this) as Rectangle).BeginAnimation(OpacityProperty, _dbani);
        }
    }

    public class BoolToVisiblilty : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
