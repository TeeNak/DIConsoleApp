using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DIConsoleApp1
{
    class Program
    {

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // Register our application entry point
            services.AddTransient<IApplication, Application>();
        }

        static async Task Main(string[] args)
        {

            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", optional: true);
                    configApp.AddJsonFile(
                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    configApp.AddEnvironmentVariables(prefix: "PREFIX_");
                    configApp.AddCommandLine(args);
                })
                .ConfigureServices(Program.ConfigureServices)
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .UseConsoleLifetime()
                .Build();

            using (host)
            {
                //await host.RunAsync();

                await host.StartAsync();

                Task shutdown = host.WaitForShutdownAsync();

                Task run = host.Services.GetService<IApplication>().RunAsync();

                Task.WaitAny(shutdown, run);

                await host.StopAsync();
            }
        }
    }
}
