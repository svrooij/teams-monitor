using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamsMonitor.Core;

namespace TeamsMonitor.Tray
{
    internal static class Program
    {
        private static IServiceProvider? _serviceProvider;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }

        internal static IServiceProvider ServiceProvider => _serviceProvider ??= ConfigureServices()!;

        private static IServiceProvider? ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            });

            services.AddSingleton<TeamsSocket>(factory =>
            {

                return new TeamsSocket(new TeamsSocketOptions()
                {
                    AutoPair = true,
                    SettingsLocation = GetDefaultStorageLocation()
                }, factory.GetRequiredService<ILogger<TeamsSocket>>());
            });

            return services.BuildServiceProvider();
        }

        private static string GetDefaultStorageLocation()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var storagePath = Path.Combine(appData, "TeamsMonitor");
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }
            return Path.Combine(storagePath, "tray.txt");
        }

    }
}