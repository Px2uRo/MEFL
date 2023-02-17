using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace MEFL.Contract{
    public abstract class MEFLClass : IDisposable
    {
        [JsonIgnore]
        public bool Disposed { get; set; }
        [JsonIgnore]
        public string FileName => Path.GetFileName(Assembly.GetAssembly(GetType()).Location);
        [JsonIgnore]
        public string AddInGuid => Assembly.GetAssembly(GetType()).ManifestModule.ModuleVersionId.ToString();

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {

                }
                Disposed = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~MEFLClass()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}