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

        // // TODO: ������Dispose(bool disposing)��ӵ�������ͷ�δ�й���Դ�Ĵ���ʱ������ս���
        // ~MEFLClass()
        // {
        //     // ��Ҫ���Ĵ˴��롣�뽫���������롰Dispose(bool disposing)��������
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}