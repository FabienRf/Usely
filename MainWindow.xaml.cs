using System;
using System.Windows;
using System.Windows.Interop;
using Usely.Core;

namespace Usely
{
    public partial class MainWindow : Window
    {
        private HotkeyManager? _hotkeyManager;

        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += MainWindow_SourceInitialized;
        }

        private void MainWindow_SourceInitialized(object? sender, EventArgs e)
        {
            _hotkeyManager = new HotkeyManager(this);
            _hotkeyManager.OnPutItOnTop += ToggleThisWindowTopmost;
        }

        private void PutItOnTop_Click(object sender, RoutedEventArgs e)
        {
            ToggleThisWindowTopmost();
        }

        private void ToggleThisWindowTopmost()
        {
            var helper = new WindowInteropHelper(this);
            var hwnd = helper.Handle;
            bool currentlyTop = WindowManager.IsWindowOnTop(hwnd);
            WindowManager.SetPutItOnTop(hwnd, !currentlyTop);
        }
    }
}
