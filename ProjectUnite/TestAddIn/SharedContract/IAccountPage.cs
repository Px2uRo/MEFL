namespace MEFL.Contract
{
    /// <summary>
    /// 仅仅用在 继承 添加用户的页面，不要 Export 这个接口
    /// </summary>
#if WPF
    public interface IAddAccountPage
#elif AVALONIA
        public interface IAddAccountContent : IDialogContent
#endif
    {
#if WPF
        public delegate void Canceled(object sender);
        public event Canceled OnCanceled;
#endif
        public delegate void AccountAdd(object sender, AccountBase account);
        public event AccountAdd OnAccountAdd;
    }

    /// <summary>
    /// 仅仅用在 构造(code) 管理用户的页面，不要 Export 这个接口
    /// </summary>
    public interface IManageAccountPage
    {
        public delegate void Canceled(object sender,AccountBase account);
        public event Canceled OnCanceled;
        public delegate void AccountDeleted(object sender, AccountBase account);
        public event AccountDeleted OnAccountDeleted;
        public delegate void Selected(object sender, AccountBase account);
        public event Selected OnSelected;
    }
}
