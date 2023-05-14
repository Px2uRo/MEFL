#if AVALONIA
using Avalonia;

namespace MEFL.Contract;
public interface IDialogContent
{
    public event EventHandler<EventArgs> Quited;
    public void WindowSizeChanged(Size size);
}
#endif