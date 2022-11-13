using System.Collections.Generic;
using MEFL.Arguments;
using MEFL.Controls;

namespace MEFL.Contract;

public interface IPages
{
    Dictionary<object, MyPageBase> IconAndPage { get; }
    void Added(int index, SettingArgs args);
    void Delected(int index, SettingArgs args);
}
