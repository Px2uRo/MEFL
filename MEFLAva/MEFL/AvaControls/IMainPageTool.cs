using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using MEFL.APIData;
using MEFL.Contract;
using MEFL.InfoControls;
using MEFL.Views;
using MEFL.Views.MainPageTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFL.AvaControls
{

    internal static class MainPageToolContoller
    {
        internal static void LoadMEFL()
        {
            if (App.Current.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime lt)
            {
                var mw = lt.MainWindow as MainWindow;
                if (APIModel.SettingConfig.ShowSimpleDownloaderTool)
                {
                    var p = APIModel.SettingConfig.SimpleDownloaderPosition;
                    var args = new AddMainPageToolArgs(new(p.X,p.Y));
                    Add(SimpleDownloaderTool.UI,"MEFL1",args);
                }
            }
        }
        public static void Add(IMainPageTool tool,string guid,AddMainPageToolArgs args)
        {
            tool.Removed -= Tool_Removed;
            tool.Removed += Tool_Removed;
            tool.Hidden -= Tool_Hidden;
            tool.Hidden += Tool_Hidden;
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var ele = new MainPageToolWindow(tool) { Tag = guid };
                ele.SetValue(Canvas.LeftProperty, args.Position.X);
                ele.SetValue(Canvas.TopProperty, args.Position.Y);
                RealMainPage.UI.ToolCanvas.Children.Add(ele);
            });
        }

        private static void Tool_Hidden(object? sender, EventArgs e)
        {

        }

        private static void Tool_Removed(object? sender, EventArgs e)
        {
            var win = (sender as Control).Parent.Parent.Parent.Parent as MainPageToolWindow;
            win.ContentBox.Content = null;
            RealMainPage.UI.ToolCanvas.Children.Remove(win);
        }

        internal static void Remove(string guid)
        {
            for(int i =0;i<RealMainPage.UI.ToolCanvas.Children.Count;i++)
            {
                MainPageToolWindow item = RealMainPage.UI.ToolCanvas.Children[i] as MainPageToolWindow;
                if (item.Tag.ToString() == guid)
                {
                    item.ContentBox.Content = null;
                    RealMainPage.UI.ToolCanvas.Children.Remove(item);
                }
            }
        }

        static MainPageToolContoller()
        {

        }
    }
}
