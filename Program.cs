using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Splat;
using System;
using System.IO;
using TWatchSKDesigner.Services;

namespace TWatchSKDesigner
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            RegisterServices();
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        private static void RegisterServices()
        {
            Bootstrap.RegisterAll(Locator.CurrentMutable);
        }

        private static string CrashLogPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TWatchSK.Designer.log");

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                File.AppendAllText(CrashLogPath, $"{DateTime.Now} failed: \r\n {e.ExceptionObject}");
            }
            catch (Exception)
            {

            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
