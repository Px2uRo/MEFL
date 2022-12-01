namespace MEFL.Contract
{
    public interface IAddAccountPage
    {
        public delegate void AccountAdd(object sender);
        public event AccountAdd OnAccountAdd;
        public AccountBase GetFinalReturn();
    }
}
