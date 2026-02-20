using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Usely.Core
{
    public class HotkeyManager : IDisposable
    {
        private const int WM_HOTKEY = 0x0312;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint VK_A = 0x41;
        private readonly IntPtr _hwnd;
        private readonly HwndSource _source;
        private const int HOTKEY_ID = 9000;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public event Action OnPutItOnTop;

        public HotkeyManager(Window window)
        {
            _hwnd = new WindowInteropHelper(window).Handle;
            _source = HwndSource.FromHwnd(_hwnd);
            _source.AddHook(HwndHook);
            RegisterHotKey(_hwnd, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, VK_A);
            window.Closed += (_, __) => Dispose();
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                OnPutItOnTop?.Invoke();
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
