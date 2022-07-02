using System;

namespace MEFL.Contract
{
#if CONTRACT&&WPF
    public interface IBaseInfo
    {
        public object Title { get; }
        public object Icon { get; }
        public Uri PulisherUri { get; }
        public Uri ExtensionUri { get; }
    }
#endif
}