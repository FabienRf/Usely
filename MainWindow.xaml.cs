using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Usely.Core;
using Usely.ViewModels;
using Usely.Views;

namespace Usely
{
    public partial class MainWindow : Window
    {
        private HotkeyManager? _hotkeyManager;
        public bool autoClick_Active = false;
        private CancellationTokenSource? _autoClickCts;

        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += MainWindow_SourceInitialized;
            DataContext = new AppSettingsHotkeys();
        }

        private void MainWindow_SourceInitialized(object? sender, EventArgs e)
        {
            _hotkeyManager = new HotkeyManager(this);
        }

        private void ToggleThisWindowTopmost()
        {
            var helper = new WindowInteropHelper(this);
            var hwnd = helper.Handle;
            bool currentlyTop = WindowManager.IsWindowOnTop(hwnd);
            WindowManager.SetPutItOnTop(hwnd, !currentlyTop);
        }

        public void ToggleAutoClicker()
        {
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
                    { }
                });
            }
            else
            {
                autoClick_Active = false;
                _autoClickCts?.Cancel();
                _autoClickCts = null;
            }
        }

        private void UpdateHotkey_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button?.DataContext is AppSettingsView.HotkeyView hotkeyView)
            {
                this.Hide();

                var updateWindow = new UpdateHotkeys(hotkeyView.ActionName, _hotkeyManager, this);
                updateWindow.ShowDialog();
            }
        }
    }
}

