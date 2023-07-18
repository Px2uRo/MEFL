using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MEFL.CLAddIn
{
    public static class ProcessUtil
    {
        // 导入Windows API函数
        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetProcessMemoryInfo(IntPtr hProcess, out PROCESS_MEMORY_COUNTERS_EX counters, int cb);

        // 定义进程内存计数器结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_MEMORY_COUNTERS_EX
        {
            public uint cb;
            public uint PageFaultCount;
            public ulong PeakWorkingSetSize;
            public ulong WorkingSetSize;
            public ulong QuotaPeakPagedPoolUsage;
            public ulong QuotaPagedPoolUsage;
            public ulong QuotaPeakNonPagedPoolUsage;
            public ulong QuotaNonPagedPoolUsage;
            public ulong PagefileUsage;
            public ulong PeakPagefileUsage;
            public ulong PrivateUsage;
        }

        public static int GetMemof(Process p)
        {
            // 打开进程句柄
            IntPtr processHandle = p.Handle;

            // 获取进程内存信息
            PROCESS_MEMORY_COUNTERS_EX counters;
            counters.cb = (uint)Marshal.SizeOf(typeof(PROCESS_MEMORY_COUNTERS_EX));
            bool success = GetProcessMemoryInfo(processHandle, out counters, (int)counters.cb);
            if (!success)
            {
                //CloseHandle(processHandle);
                throw new InvalidOperationException("无法获取进程内存信息。错误码：" + Marshal.GetLastWin32Error());
            }

            // 获取进程的工作集大小
            ulong memorySize = counters.WorkingSetSize;
            int res = (int)(memorySize / 1024 / 1024);
            GC.SuppressFinalize(memorySize);
            GC.SuppressFinalize(counters);
            // 关闭进程句柄
            //CloseHandle(processHandle)

            return res;
        }

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
    }

}
