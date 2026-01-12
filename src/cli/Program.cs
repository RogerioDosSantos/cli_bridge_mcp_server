using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CliBridgeMCPServer.Commands;
using CliBridgeMCPServer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CliBridgeMCPServer
{
    class Program
    {
        private static async Task<int> Main(string[] args)
        {

            // Configuration
            IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "\\appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Dependency Injection
            IHostBuilder builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(config =>
                    {
                        config.AddConsole();
                    });
                    services.AddHttpClient();
                });

            // Run Application
            try
            {
                return await builder.RunCommandLineApplicationAsync<MainCommand>(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not execute the {args[0]} command.");
                Console.WriteLine($"This can happen due to invalid parameters.");
                Console.WriteLine($"Call the {args[0]} --help for information about the expected parameters.");
                Console.WriteLine($"Call Command:");
                Console.WriteLine(string.Join(" ", args));
                Console.WriteLine($"Error Details:");
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
    }
}
