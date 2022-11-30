namespace MEFL.Contract
{
    public interface IAddAccountPage
    {
        public delegate void AccountAdded(object sender);
        public event AccountAdded OnAccountAdded;
        public AccountBase GetFinalReturn();
    }
}
