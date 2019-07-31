using System;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleTemplate
{
    class Program
    {
        private const string _prefix = "MYAPP_";
        private const string _appsettings = "appsettings.json";
        private const string _hostsettings = "hostsettings.json";
        
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile(_hostsettings, optional: true);
                    configHost.AddEnvironmentVariables(prefix: _prefix);
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile(_appsettings, optional: true);
                    configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    configApp.AddEnvironmentVariables(prefix: _prefix);
                    configApp.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) => { services.AddLogging(); })
                .ConfigureLogging((hostContext, configLogging) => { configLogging.AddConsole(); })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
            Console.WriteLine("Hello World!");
        }
    }
}
