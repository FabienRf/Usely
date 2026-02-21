using System;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Interop;
using Usely.Core;

namespace Usely
{
    public partial class MainWindow : Window
    {
        private HotkeyManager? _hotkeyManager;

        // Window constructor
        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += MainWindow_SourceInitialized;
        }

        // Called when the native window is ready; set up hotkeys
        private void MainWindow_SourceInitialized(object? sender, EventArgs e)
        {
            _hotkeyManager = new HotkeyManager(this);
        }

        // Button handler: toggle this app window's topmost state
        private void PutItOnTop_Click(object sender, RoutedEventArgs e)
        {
            ToggleThisWindowTopmost();
        }

        // Toggle the "always on top" flag for this window
        private void ToggleThisWindowTopmost()
        {
            var helper = new WindowInteropHelper(this);
            var hwnd = helper.Handle;
            bool currentlyTop = WindowManager.IsWindowOnTop(hwnd);
            WindowManager.SetPutItOnTop(hwnd, !currentlyTop);
        }


        // Hotkey handling is done by HotkeyManager in Core/HotkeyManager.cs
    }
}

