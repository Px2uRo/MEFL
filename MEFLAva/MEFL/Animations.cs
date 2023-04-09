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
        public static Animation _anigo;
        public static Animation _aniback;
        private static Easing _easing = new CircularEaseInOut();

        public static Animation AniGo
        {
            get
            {
                if (_anigo == null)
                {
                    _anigo = new Animation()
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
                        Value = new Thickness(0, 0, 0, 0)
                    });
                    _anigo.Children.Add(kfr);
                }
                return _anigo;
            }
        }
        public static Animation AniBack
        {
            get
            {
                if (_aniback == null)
                {
                    _aniback = new Animation()
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
                        Value = new Thickness(30, 0, 0, 0)
                    });
                    _aniback.Children.Add(kfr);
                }
                return _aniback;
            }
        }

        public static Animation _widthgo;
        public static Animation _widthback;

        public static Animation WidthGo
        {
            get
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
                        Value = 258.0
                    });
                    _widthgo.Children.Add(kfr);
                }
                return _widthgo;
            }
        }
        public static Animation WidthBack
        {
            get
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
                        Value = 288.0
                    });
                    _widthback.Children.Add(kfr);
                }
                return _widthback;
            }
        }
        #endregion
    }
}
