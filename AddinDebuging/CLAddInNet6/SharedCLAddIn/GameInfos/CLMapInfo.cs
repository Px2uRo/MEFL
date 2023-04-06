using CoreLaunching.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
#if WPF
using System.Windows.Media;
using System.Windows.Media.Imaging;
#elif AVALONIA
using Avalonia.Media;
using Avalonia.Media.Imaging;
#endif

namespace MEFL.CLAddIn.GameInfos
{
    internal class CLMapInfo:MapInfo
    {
        public override string ToString()
        {
            return Name;
        }
        public CLMapInfo(string folder) : base(folder)
        {

        }

#if WPF
public ImageSource Image {get
                {
                var path = Path.Combine(Folder, IconName);
                if (!File.Exists(path)) return null;
                else return new BitmapImage(new Uri(path));
            } 
        }
#elif AVALONIA
        public IImage Image
        {
            get
            {
                var path = Path.Combine(Folder, IconName);
                if (!File.Exists(path)) return null;
                else return new Bitmap(path);
            }
        }
#endif
    }

    internal class GetAllMaps
    {
        public static CLMapInfo[] FromFolder(string saves)
        {
            var rel = new List<CLMapInfo>();
            Directory.CreateDirectory(saves);
            for (int i = 0; i < Directory.GetDirectories(saves).Count(); i++)
            {
                var save = Directory.GetDirectories(saves)[i];
                rel.Add(new CLMapInfo(save));
            }
            return rel.ToArray();
        }
    }
}
