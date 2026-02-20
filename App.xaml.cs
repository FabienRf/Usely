using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Usely
{
    public partial class App : Application
    {
        private string _logPath = Path.Combine(AppContext.BaseDirectory, "usely-error.log");

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LogException(e.Exception);
            MessageBox.Show("Une erreur inattendue est survenue. Voir usely-error.log.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                LogException(ex);
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            LogException(e.Exception);
            e.SetObserved();
        }

        private void LogException(Exception ex)
        {
            try
            {
                File.AppendAllText(_logPath, $"[{DateTime.Now}] {ex}\n\n");
            }
            catch { }
        }
    }
}
