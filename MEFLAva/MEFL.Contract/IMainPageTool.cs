using Avalonia;
using System.ComponentModel;

namespace MEFL.Contract
{
    public enum MainPageToolStyle
    {
        AlwaysShowBorder =0,
        CanHideBorder =1,
        NoBorder =2
    }
    public interface IMainPageTool:INotifyPropertyChanging
    {
        public MainPageToolStyle GetStyle();
        public bool IsAbleToExit { get; set; }
        public string Title { get; set; }
        public void ChangePosition(Point position);
        public void Remove();

        public event EventHandler<Point> OnPositionChanged;
        public event EventHandler<EventArgs> Removed;
        public event EventHandler<EventArgs> Hidden;
    }

    public class AddMainPageToolArgs : EventArgs
    {
        public Point Position { get; private set; }
        public AddMainPageToolArgs(Point position) 
        { 
            Position= position;
        }
    }
}
