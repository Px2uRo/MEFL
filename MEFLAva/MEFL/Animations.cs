using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFL
{
    internal static class Animations
    {
        #region Animation
        public static Animation _margingo;
        public static Animation _marginback;
        private static Easing _easing = new CircularEaseInOut();

        public static Animation MarginGo(Thickness margin)
        {
                if (_margingo == null)
                {
                    _margingo = new Animation()
                    {
                        Duration = TimeSpan.FromSeconds(0.2),
                        PlaybackDirection = PlaybackDirection.Normal,
                        FillMode = FillMode.Both,
                        Easing = _easing
                    };
                    var kfr = new KeyFrame()
                    {
                        Cue = new(1.0)
                    };
                    kfr.Setters.Add(new Setter()
                    {
                        Property = Button.MarginProperty,
                        Value = margin
                    });
                    _margingo.Children.Add(kfr);
                }
            return _margingo;
        }
        public static Animation MarginBack(Thickness margin)
        {
                if (_marginback == null)
                {
                    _marginback = new Animation()
                    {
                        Duration = TimeSpan.FromSeconds(0.2),
                        PlaybackDirection = PlaybackDirection.Normal,
                        FillMode = FillMode.Both,
                        Easing = _easing
                    };
                    var kfr = new KeyFrame()
                    {
                        Cue = new(1.0)
                    };
                    kfr.Setters.Add(new Setter()
                    {
                        Property = Button.MarginProperty,
                        Value = margin
                    });
                    _marginback.Children.Add(kfr);
                }
                return _marginback;
            
        }

        public static Animation _widthgo;
        public static Animation _widthback;

        public static Animation WidthGo(double width)
        {
                if (_widthgo == null)
                {
                    _widthgo = new Animation()
                    {
                        Duration = TimeSpan.FromSeconds(0.2),
                        PlaybackDirection = PlaybackDirection.Normal,
                        FillMode = FillMode.Both,
                        Easing = _easing
                    };
                    var kfr = new KeyFrame()
                    {
                        Cue = new(1.0)
                    };
                    kfr.Setters.Add(new Setter()
                    {
                        Property = Button.WidthProperty,
                        Value = width
                    });
                    _widthgo.Children.Add(kfr);
                }
                return _widthgo;
        }
        public static Animation WidthBack(double width)
        {
                if (_widthback == null)
                {
                    _widthback = new Animation()
                    {
                        Duration = TimeSpan.FromSeconds(0.2),
                        PlaybackDirection = PlaybackDirection.Normal,
                        FillMode = FillMode.Both,
                        Easing = _easing
                    };
                    var kfr = new KeyFrame()
                    {
                        Cue = new(1.0)
                    };
                    kfr.Setters.Add(new Setter()
                    {
                        Property = Button.WidthProperty,
                        Value = width
                    });
                    _widthback.Children.Add(kfr);
                }
                return _widthback;
        }
        #endregion
    }
}
