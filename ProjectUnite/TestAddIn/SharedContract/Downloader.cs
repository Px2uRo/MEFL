namespace MEFL.Contract
{
    public abstract class MEFLDownloader:MEFLClass
    {
        public abstract void Download(string url,string loacl);
    }
}