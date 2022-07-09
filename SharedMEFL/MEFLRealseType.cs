using MEFL.Arguments;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL
{
    public class MEFLRealseType : MEFL.Contract.GameInfoBase
    {
        private string _Name { get; set; }
        public override string GameTypeFriendlyName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Version { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Name { get => _Name; set { _Name = value; } }
        public override object IconSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Refresh(SettingArgs args)
        {
            throw new NotImplementedException();
        }

        public MEFLRealseType()
        {
            _Name = "???";
        }
    }
}
