using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Interop;
using Usely;

namespace Usely.Core
{
    public class HotkeyManager : IDisposable
    {
        private const int WM_HOTKEY = 0x0312;
        private const uint MOD_CTRL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;

        private readonly MainWindow _mainWindow;
        private readonly IntPtr _hwnd;
        private readonly HwndSource _source;

        private readonly Dictionary<int, string> _hotkeyActions = new();
        private int _nextHotkeyId = 1;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public HotkeyManager(MainWindow window)
        {
            _mainWindow = window;

            _hwnd = new WindowInteropHelper(window).Handle;
            _source = HwndSource.FromHwnd(_hwnd);
            _source.AddHook(HwndHook);

            RegisterConfiguredHotkeys();

            window.Closed += (_, __) => Dispose();
        }

        private void RegisterConfiguredHotkeys()
        {
            try
            {
                var baseDir = AppContext.BaseDirectory;
                var settingsPath = Path.Combine(baseDir, "appsettings.json");

                if (!File.Exists(settingsPath))
                {
                    RegisterDefaultPutItOnTop();
                    return;
                }

                string json = File.ReadAllText(settingsPath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var config = JsonSerializer.Deserialize<Root>(json, options);
                if (config?.Hotkeys == null || config.Hotkeys.Count == 0)
                {
                    RegisterDefaultPutItOnTop();
                    return;
                }

                foreach (var item in config.Hotkeys)
                {
                    var actionName = item.Key;
                    var hotkey = item.Value;

                    if (hotkey == null || string.IsNullOrWhiteSpace(hotkey.WindowActionKey))
                        continue;

                    if (!TryParseHexVk(hotkey.WindowActionKey, out uint vk))
                        continue;

                    int id = _nextHotkeyId++;
                    if (RegisterHotKey(_hwnd, id, MOD_CTRL | MOD_SHIFT, vk))
                    {
                        _hotkeyActions[id] = actionName;
                    }
                }

                if (_hotkeyActions.Count == 0)
                {
                    RegisterDefaultPutItOnTop();
                }
            }
            catch
            {
                RegisterDefaultPutItOnTop();
            }
        }

        private void RegisterDefaultPutItOnTop()
        {
            const uint VK_UP = 0x26;
            const string actionName = "PutItOnTop";

            int id = _nextHotkeyId++;
            if (RegisterHotKey(_hwnd, id, MOD_CTRL | MOD_SHIFT, VK_UP))
            {
                _hotkeyActions[id] = actionName;
            }
        }

        private static bool TryParseHexVk(string value, out uint vk)
        {
            value = value.Trim();
            if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Substring(2);
            }

            return uint.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out vk);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY)
            {
                int id = wParam.ToInt32();
                if (_hotkeyActions.TryGetValue(id, out var actionName))
                {
                    try
                    {
                        HandleHotkey(actionName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error handling hotkey '{actionName}': {ex.Message}");
                    }

                    handled = true;
                }
            }

            return IntPtr.Zero;
        }

        private void HandleHotkey(string actionName)
        {
            switch (actionName)
            {
                case "PutItOnTop":
                    HandlePutItOnTop();
                    break;
                case "Capture":
                    HandleCapture();
                    break;
                case "DrawMode":
                    HandleDrawMode();
                    break;
                case "AutoClicker":
                    HandleAutoClicker();
                    break;
                default:
                    break;
            }
        }

        private void HandlePutItOnTop()
        {
            var target = WindowManager.GetForegroundWindow();
            if (target != IntPtr.Zero && target != _hwnd)
            {
                bool currentlyTop = WindowManager.IsWindowOnTop(target);
                WindowManager.SetPutItOnTop(target, !currentlyTop);
            }
        }

        private void HandleCapture()
        {
            // Skeleton
        }

        private void HandleDrawMode()
        {
            // Skeleton
        }

        private void HandleAutoClicker()
        {
            _mainWindow.ToggleAutoClicker();
        }

        public void Dispose()
        {
            foreach (var id in _hotkeyActions.Keys)
            {
                UnregisterHotKey(_hwnd, id);
            }

            if (_source != null)
                _source.RemoveHook(HwndHook);
        }
    }
}
