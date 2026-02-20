using System;
using System.Runtime.InteropServices;

namespace Usely.Core
{
    public class WindowManager
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOPMOST = 0x00000008;
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        public static void SetPutItOnTop(IntPtr windowHandle, bool top)
        {
            SetWindowPos(windowHandle,
                top ? HWND_TOPMOST : HWND_NOTOPMOST,
                0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE);
        }

        public static bool IsWindowOnTop(IntPtr windowHandle)
        {
            return (GetWindowLong(windowHandle, GWL_EXSTYLE) & WS_EX_TOPMOST) != 0;
        }
    }
}

