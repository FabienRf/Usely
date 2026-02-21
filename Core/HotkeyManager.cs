using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Usely.Core
{
    public class HotkeyManager : IDisposable
    {
        // Hotkey message ID
        private const int WM_HOTKEY = 0x0312;
        private const uint MOD_CTRL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;

        // Up arrow key
        private const uint VK_UP = 0x26;
        // Native window handle
        private readonly IntPtr _hwnd;
        // Message source/hook
        private readonly HwndSource _source;

        // Hotkey identifier
        private const int HOTKEY_ID = 1;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public HotkeyManager(Window window)
        {
            _hwnd = new WindowInteropHelper(window).Handle;
            _source = HwndSource.FromHwnd(_hwnd);
            _source.AddHook(HwndHook);
            RegisterHotKey(_hwnd, HOTKEY_ID, MOD_CTRL | MOD_SHIFT, VK_UP);
            window.Closed += (_, __) => Dispose();
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                try
                {
                    var target = WindowManager.GetForegroundWindow();
                    if (target != IntPtr.Zero && target != _hwnd)
                    {
                        bool currentlyTop = WindowManager.IsWindowOnTop(target);
                        WindowManager.SetPutItOnTop(target, !currentlyTop);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error handling hotkey: {ex.Message}");
                }
                handled = true;
            }
            return IntPtr.Zero;
        }

        public void Dispose()
        {
            UnregisterHotKey(_hwnd, HOTKEY_ID);
            if (_source != null)
                _source.RemoveHook(HwndHook);
        }
    }
}
