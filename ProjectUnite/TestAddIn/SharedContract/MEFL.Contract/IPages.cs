using System.Collections.Generic;
using MEFL.Controls;

namespace MEFL.Contract;

public interface IPages
{
	Dictionary<object, MyPageBase> IconAndPage { get; }
}
