using System;
using Avalonia;
using Avalonia.ReactiveUI;
using EmployeeManager.Desktop.Services;
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
                    services.AddHttpClient<EmployeeApiService>(client =>
                    {
                        client.BaseAddress = new Uri("https://localhost:7240/api/");
                    });

                    services.AddHttpClient<CompanyApiService>(client =>
                    {
                        client.BaseAddress = new Uri("https://localhost:7240/api/");
                    });
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .Build();
        }
    }
}
