using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFL.AvaControls
{
    public interface IMainPageTool
    {
        public event EventHandler<EventArgs> Removed;
        public event EventHandler<EventArgs> Hidden;
    }

    public static class MainPageToolBox
    {
        public static void Add(IMainPageTool tool)
        {
            tool.Removed -= Tool_Removed;
            tool.Removed += Tool_Removed;
            tool.Hidden -= Tool_Hidden;
            tool.Hidden += Tool_Hidden;
        }

        private static void Tool_Hidden(object? sender, EventArgs e)
        {

        }

        private static void Tool_Removed(object? sender, EventArgs e)
        {

        }
    }
}
