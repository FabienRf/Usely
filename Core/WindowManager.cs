using System;
using System.Runtime.InteropServices;

namespace Usely.Core
{
    public class WindowManager
    {
        // Native constants for window style and positioning
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOPMOST = 0x00000008;
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;

        // SetWindowPos moves or changes window z-order (used to set topmost)
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        // GetWindowLong reads window styles (used to check topmost flag)
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        // Returns the handle of the window that currently has focus
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        // Makes the given window topmost (always on top) or restores it
        public static void SetPutItOnTop(IntPtr windowHandle, bool top)
        {
            SetWindowPos(windowHandle,
                top ? HWND_TOPMOST : HWND_NOTOPMOST,
                0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE);
        }

        // Returns true if the window currently has the topmost flag
        public static bool IsWindowOnTop(IntPtr windowHandle)
        {
            return (GetWindowLong(windowHandle, GWL_EXSTYLE) & WS_EX_TOPMOST) != 0;
        }
    }
}

