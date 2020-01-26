using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace ImageBrowse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Application start event handler
        /// protected override void OnStartup(StartupEventArgs e)
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            DispatcherUnhandledException += Application_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// Application end event handler
        /// protected override void OnExit(ExitEventArgs e)
        /// </summary>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            DispatcherUnhandledException -= Application_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// Event handler when throwing unhandled exception in WPF UI thread
        /// </summary>
        private void Application_DispatcherUnhandledException(
            object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ReportUnhandledException(e.Exception);
        }

        /// <summary>
        /// Event handler for throwing unhandled exceptions other than UI thread
        /// </summary>
        private void CurrentDomain_UnhandledException(
            object sender, UnhandledExceptionEventArgs e)
        {
            ReportUnhandledException(e.ExceptionObject as Exception);
        }

        /// <summary>
        /// Output unhandled exception to event log.
        /// </summary>
        private void ReportUnhandledException(Exception ex)
        {
            string nowDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string logNowDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string appendText = $"{nowDate}:{ex.ToString()}{Environment.NewLine}";
            File.AppendAllText($@"{logNowDate}_errorlog.txt", appendText);
            MessageBox.Show($@"Exit due to error。{Environment.NewLine}" +
                $@"The error log is output to {Environment.NewLine}{Directory.GetCurrentDirectory()}\{logNowDate}_errorlog.txt{Environment.NewLine}");
            Shutdown();
        }
    }
}
