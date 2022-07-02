using MEFL.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TestAddIn
{
    class TestGameType : MEFL.Contract.GameInfoBase
    {
        public override string GameTypeFriendlyName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Version { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override ImageSource IconSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Refresh(SettingArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
