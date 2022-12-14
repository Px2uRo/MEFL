using System;
using System.Reflection;

namespace MEFL.Contract{
    public abstract class MEFLClass : IDisposable
    {
        public bool Disposed { get; set; }

        public string AddInGuid => Assembly.GetAssembly(GetType()).ManifestModule.ModuleVersionId.ToString();

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    // TODO: �ͷ��й�״̬(�йܶ���)
                }

                // TODO: �ͷ�δ�йܵ���Դ(δ�йܵĶ���)����д�ս���
                // TODO: �������ֶ�����Ϊ null
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
            // ��Ҫ���Ĵ˴��롣�뽫���������롰Dispose(bool disposing)��������
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}