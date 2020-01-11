using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace ImageBrowse
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// アプリケーション開始時のイベントハンドラprotected override void OnStartup(StartupEventArgs e)
        /// (App.xamlで定義)
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            DispatcherUnhandledException += Application_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// アプリケーション終了時のイベントハンドラprotected override void OnExit(ExitEventArgs e)
        /// </summary>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            DispatcherUnhandledException -= Application_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// WPF UIスレッドでの未処理例外スロー時のイベントハンドラ
        /// </summary>
        private void Application_DispatcherUnhandledException(
            object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ReportUnhandledException(e.Exception);
        }

        /// <summary>
        /// UIスレッド以外の未処理例外スロー時のイベントハンドラ
        /// </summary>
        private void CurrentDomain_UnhandledException(
            object sender, UnhandledExceptionEventArgs e)
        {
            ReportUnhandledException(e.ExceptionObject as Exception);
        }

        /// <summary>
        /// 未処理例外をイベントログに出力します。
        /// </summary>
        private void ReportUnhandledException(Exception ex)
        {
            string nowDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string logNowDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string appendText = $"{nowDate}:{ex.ToString()}{Environment.NewLine}";
            File.AppendAllText($@"{logNowDate}_errorlog.txt", appendText);
            MessageBox.Show($@"エラーが発生したため終了します。{Environment.NewLine}" +
                $@"エラーログは{Environment.NewLine}{Directory.GetCurrentDirectory()}\{logNowDate}_errorlog.txt{Environment.NewLine}に出力しています。");
            Shutdown();
        }
    }
}
