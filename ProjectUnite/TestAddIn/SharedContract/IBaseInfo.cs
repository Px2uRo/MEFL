using System;

namespace MEFL.Contract
{
#if CONTRACT&&WPF
    public interface IBaseInfo
    {
#if NET4_0
        object Title { get; }
        object Icon { get; }
        Uri PulisherUri { get; }
        Uri ExtensionUri { get; }

#else
        public object Title { get; }
        public object Icon { get; }
        public Uri PulisherUri { get; }
        public Uri ExtensionUri { get; }
#endif
    }
#endif
}
