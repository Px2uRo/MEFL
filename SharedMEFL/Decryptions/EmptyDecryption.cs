using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Decryptions
{
    ///<summary>屁用没有的加密器。</summary>
    public class EmptyDecryption : BaseDecryption
    {
        public override string Encrypt(string source)
        {
            return source;
        }

        public override string Decrypt(string passwordSource)
        {
            return passwordSource;
        }
    }
}
