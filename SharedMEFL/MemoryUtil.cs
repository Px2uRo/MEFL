using CZGL.SystemInfo;
using System;
using System.Runtime.InteropServices;

namespace MEFL
{
    public static class MemoryUtil
    {
        public static ulong GetTotal()
        {
            using (var api = new CppAPI())
            {
                return api.GetTotal();
            }
        }
        public static ulong GetFree()
        {
            using (var api = new CppAPI())
            {
                return api.GetFree();
            }
        }
    }

    public class CppAPI : IDisposable
    {
        #region APIs
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);
        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }
        [DllImport("libc")]
        private static extern int sysinfo(ref sysinfo_t info);

        [StructLayout(LayoutKind.Sequential)]
        private struct sysinfo_t
        {
            public long uptime;          // 系统启动时间
            public ulong loads_1;        // 1分钟平均负载
            public ulong loads_5;        // 5分钟平均负载
            public ulong loads_15;       // 15分钟平均负载
            public ulong totalram;       // 总内存大小
            public ulong freeram;        // 可用内存大小
            public ulong sharedram;      // 共享内存大小
            public ulong bufferram;      // 缓冲区内存大小
            public ulong totalswap;      // 总交换空间大小
            public ulong freeswap;       // 可用交换空间大小
            public ushort procs;         // 进程数量
            public ushort pad;           // 未使用
            public ulong totalhigh;      // 高内存总大小
            public ulong freehigh;       // 可用高内存大小
            public uint mem_unit;        // 内存单位大小
            public IntPtr _f;            // 未使用
        }
        #endregion

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                GC.SuppressFinalize(win_memoryStatus);
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~WindowsMemoryAPI()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        MEMORYSTATUSEX win_memoryStatus;

        public ulong GetTotal()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                win_memoryStatus = new MEMORYSTATUSEX();
                win_memoryStatus.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
                GlobalMemoryStatusEx(ref win_memoryStatus);
                return win_memoryStatus.ullTotalPhys / 1024 / 1024;
            }
            sysinfo_t info = new sysinfo_t();
            sysinfo(ref info);
            return info.totalram;
        }
        public ulong GetFree()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                MEMORYSTATUSEX memoryStatus = new MEMORYSTATUSEX();
                memoryStatus.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
                GlobalMemoryStatusEx(ref memoryStatus);
                return memoryStatus.ullAvailPhys / 1024 / 1024;
            }
            sysinfo_t info = new sysinfo_t();
            sysinfo(ref info);
            return info.freeram;
        }

    }
}