using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Decryptions
{
    public abstract class BaseDecryption
    {
        ///<summary>加密。</summary>
        public virtual string Encrypt(string source)
        {
            throw new NotImplementedException();
        }

        ///<summary>解密</summary>
        ///<param name="passwordSource">加密后的文本。</param>
        public virtual string Decrypt(string passwordSource)
        {
            throw new NotImplementedException();
        }
    }
}
