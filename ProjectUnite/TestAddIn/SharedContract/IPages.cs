using System.Collections.Generic;
using MEFL.Arguments;
#if WPF
using MEFL.Controls;
#elif AVALONIA
using Avalonia.Controls;
#endif

namespace MEFL.Contract;

public interface IPages
{
#if WPF
    Dictionary<object, MyPageBase> IconAndPage { get; }
#elif AVALONIA
    Dictionary<object, Control> IconAndPage { get; }
#endif
    void Added(int index, SettingArgs args);
    void Delected(int index, SettingArgs args);
}
