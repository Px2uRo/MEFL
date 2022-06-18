using System.Collections.Generic;

namespace MEFL.Contract
{
#if WPF
    public interface IPages
    {
        public Dictionary<object,MEFL.Controls.MyPageBase> IconAndPage{ get; }
    }
#endif
}
