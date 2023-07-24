using CLAddIn.Properties;
using System;
using System.Collections.Generic;
using System.Text;
#if AVALONIA
using Bitmap = Avalonia.Media.Imaging.Bitmap;
#endif

namespace MEFL.CLAddIn
{
    internal static class BitMaps
    {
        #region Statics
        #endregion

        private static Bitmap _release;
        private static Bitmap _beta;
        private static Bitmap _alpha;
        private static Bitmap _forgeRelease;
        private static Bitmap _forgeBeta;
        private static Bitmap _forgeAlpha;
        private static Bitmap _fabricRelease;
        private static Bitmap _fabricBeta;
        private static Bitmap _fabricAlpha;
        private static Bitmap _resourceFinder;
        private static Bitmap _imageError;
        #region Bitmaps
        public static Bitmap ImageError
        {
            get
            {
                if (_imageError == null)
                {

                    using (var ms = new MemoryStream())
                    {
                        Resources.ImageError.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _resourceFinder = new Bitmap(ms);
                    }
                }
                return _imageError;
            }

        }
        public static Bitmap ResourceFinder
        {
            get {
                if (_resourceFinder == null) {

                    using (var ms = new MemoryStream())
                    {
                        Resources.ResourceFinder.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _resourceFinder = new Bitmap(ms);
                    }
                }
                return _resourceFinder;
            }
        }

        public static Bitmap Release
        {
            get { 
                if(_release == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.Release.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _release = new Bitmap(ms);
                    }
                }
                return _release;
            }
        }
        public static Bitmap Beta
        {
            get
            {
                if (_beta == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.Beta.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _beta = new Bitmap(ms);
                    }
                }
                return _beta;
            }
        }
        public static Bitmap Alpha
        {
            get
            {
                if (_alpha == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.Alpha.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _alpha = new Bitmap(ms);
                    }
                }
                return _alpha;
            }
        }
        public static Bitmap ForgeRelease
        {
            get
            {
                if (_forgeRelease == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.ForgeRelease.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _forgeRelease = new Bitmap(ms);
                    }
                }
                return _forgeRelease;
            }
        }
        public static Bitmap ForgeBeta
        {
            get
            {
                if (_forgeBeta == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.ForgeBeta.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _forgeBeta = new Bitmap(ms);
                    }
                }
                return _forgeBeta;
            }
        }
        public static Bitmap ForgeAlpha
        {
            get
            {
                if (_forgeAlpha == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.ForgeAlpha.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _forgeAlpha = new Bitmap(ms);
                    }
                }
                return _forgeAlpha;
            }
        }
        public static Bitmap FabricRelease
        {
            get
            {
                if (_fabricRelease == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.FabricRelease.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _fabricRelease = new Bitmap(ms);
                    }
                }
                return _fabricRelease;
            }
        }
        public static Bitmap FabricBeta
        {
            get
            {
                if (_fabricBeta == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.FabricBeta.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _fabricBeta = new Bitmap(ms);
                    }
                }
                return _fabricBeta;
            }
        }
        public static Bitmap FabricAlpha
        {
            get
            {
                if (_fabricAlpha == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.FabricAlpha.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        _fabricAlpha = new Bitmap(ms);
                    }
                }
                return _fabricAlpha;
            }
        }

        #endregion
    }
}
