using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Microsoft.DataTransfer.WpfHost.Model
{
    sealed class TaskBarService : ITaskBarService
    {
        private WindowInteropHelper mainWindow;

        public TaskBarService(IApplicationController applicationController)
        {
            mainWindow = new WindowInteropHelper(applicationController.GetMainWindow());
        }

        public bool Notify()
        {
            var fInfo = new NativeMethods.FLASHWINFO
            {
                cbSize = Convert.ToUInt32(Marshal.SizeOf(typeof(NativeMethods.FLASHWINFO))),
                hwnd = mainWindow.Handle,
                dwFlags = NativeMethods.FLASHW_TRAY | NativeMethods.FLASHW_TIMERNOFG,
                uCount = 2,
                dwTimeout = 0
            };

            return !NativeMethods.FlashWindowEx(ref fInfo);
        }

        private static class NativeMethods
        {
            public const UInt32 FLASHW_TRAY = 2;
            public const UInt32 FLASHW_TIMERNOFG = 12;

            [StructLayout(LayoutKind.Sequential)]
            public struct FLASHWINFO
            {
                public UInt32 cbSize;
                public IntPtr hwnd;
                public UInt32 dwFlags;
                public UInt32 uCount;
                public UInt32 dwTimeout;
            }

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);
        }
    }
}
