using MEFL.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if AVALONIA
using Bitmap = Avalonia.Media.Imaging.Bitmap;
#endif

namespace MEFL
{
    internal static class BitMaps
    {
        #region Statics
        

        private static Bitmap _homepage;
        private static Bitmap _downloads;
        private static Bitmap _addins;
        private static Bitmap _settings;
        private static Bitmap _downloadings;
        private static Bitmap _running;
        #endregion

        #region Bitmaps
        public static Bitmap HomePage
        {
            get
            {
                if (_homepage == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.HomePage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _homepage = new Bitmap(ms);
                    }
                }
                return _homepage;
            }
        }
        public static Bitmap Downloads
        {
            get
            {
                if (_downloads == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.Downloads.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _downloads = new Bitmap(ms);
                    }
                }
                return _downloads;
            }
        }
        public static Bitmap AddIns
        {
            get
            {
                if (_addins == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.Addins.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _addins = new Bitmap(ms);
                    }
                }
                return _addins;
            }
        }
        public static Bitmap Settings
        {
            get
            {
                if (_settings == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.Settings.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _settings = new Bitmap(ms);
                    }
                }
                return _settings;
            }
        }
        public static Bitmap Downloadings
        {
            get
            {
                if (_downloadings == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.Downloadings.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _downloadings = new Bitmap(ms);
                    }
                }
                return _downloadings;
            }
        }
        public static Bitmap Running
        {
            get
            {
                if (_running == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.Runnings.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _running = new Bitmap(ms);
                    }
                }
                return _running;
            }
        }

        #endregion
    }
}
