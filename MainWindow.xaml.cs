using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Usely.Core;
using Usely.ViewModels;

namespace Usely
{
    public partial class MainWindow : Window
    {
        private HotkeyManager? _hotkeyManager;
        public bool autoClick_Active = false;
        private CancellationTokenSource? _autoClickCts;

        // Window constructor
        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += MainWindow_SourceInitialized;
            DataContext = new AppSettingsHotkeys();
        }

        // Called when the native window is ready; set up hotkeys
        private void MainWindow_SourceInitialized(object? sender, EventArgs e)
        {
            _hotkeyManager = new HotkeyManager(this);
        }

        // Toggle the "always on top" flag for this window
        private void ToggleThisWindowTopmost()
        {
            var helper = new WindowInteropHelper(this);
            var hwnd = helper.Handle;
            bool currentlyTop = WindowManager.IsWindowOnTop(hwnd);
            WindowManager.SetPutItOnTop(hwnd, !currentlyTop);
        }

        public void ToggleAutoClicker()
        {
            // Start autoclicker
            if (!autoClick_Active)
            {
                autoClick_Active = true;
                _autoClickCts = new CancellationTokenSource();
                var token = _autoClickCts.Token;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        while (!token.IsCancellationRequested)
                        {
                            MouseClicker.LeftClick();
                            await Task.Delay(200, token);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        // expected on stop
                    }
                });
            }
            // Stop autoclicker
            else
            {
                autoClick_Active = false;
                _autoClickCts?.Cancel();
                _autoClickCts = null;
            }
        }
    }
}

