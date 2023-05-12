using Avalonia.Controls;
using MEFL.Contract;

namespace CLAddIn.Views
{
    public partial class MEFLRealseTypeSetting : UserControl,IGameSettingPage
    {
        public MEFLRealseTypeSetting()
        {
            InitializeComponent();
        }

        public event EventHandler<GameInfoBase> OnSelected;
        public event EventHandler<GameInfoBase> OnRemoved;
        public event EventHandler<GameInfoBase> OnPageBack;
        public event EventHandler<GameInfoBase> OnListUpdate;
        public event EventHandler<EventArgs> Quited;
    }
}
