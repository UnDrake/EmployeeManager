using System;
using Avalonia;
using Avalonia.ReactiveUI;
using EmployeeManager.Desktop.Services;
using EmployeeManager.Desktop.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Desktop
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();
        }

        public static IHost BuildHost()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var baseAddress = context.Configuration.GetValue<string>("ApiBaseUrl") ?? "https://localhost:7240/api/";

                    services.AddHttpClient<EmployeeApiService>(client =>
                    {
                        client.BaseAddress = new Uri(baseAddress);
                    });

                    services.AddHttpClient<CompanyApiService>(client =>
                    {
                        client.BaseAddress = new Uri(baseAddress);
                    });

                    services.AddSingleton<ReportSaver>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .Build();
        }
    }
}
