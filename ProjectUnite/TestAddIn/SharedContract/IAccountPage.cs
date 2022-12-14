using System.Windows.Controls;

namespace MEFL.Contract
{
    /// <summary>
    /// 仅仅用在 构造(code) 添加用户的页面，不要 Export 这个接口
    /// </summary>
    public interface IAddAccountPage
    {
        public delegate void Canceled(object sender);
        public event Canceled OnCanceled;
        public delegate void AccountAdd(object sender);
        public event AccountAdd OnAccountAdd;
        public virtual AccountBase GetFinalReturn()
        {
            return null;
        }
    }

    /// <summary>
    /// 仅仅用在 构造(code) 管理用户的页面，不要 Export 这个接口
    /// </summary>
    public interface IAddManagetPage
    {
        public delegate void Canceled(object sender);
        public event Canceled OnCanceled;
        public delegate void AccountEnabled(object sender);
        public event AccountEnabled OnAccountEnabled;
        public delegate void Selected(object sender);
        public event Selected OnSelected;
    }
}
