using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.RegManagers
{
    public abstract class BaseRegManager
    {

        ///<summary>当前注册管理器的加解密工具</summary>
        protected Decryptions.BaseDecryption _decryption;

        public BaseRegManager(Decryptions.BaseDecryption decryption)
        {
            _decryption = decryption;
        }

        public BaseRegManager():this(null)
        {
            if (_decryption == null) _decryption = new Decryptions.EmptyDecryption();
        }

        public virtual void Write(string key, string value, bool forceWrite)
        {
            throw new NotImplementedException("请实现接口。");
        }

        public virtual string Read(string key)
        {
            throw new NotImplementedException("请实现接口。");
        }
    }
}
