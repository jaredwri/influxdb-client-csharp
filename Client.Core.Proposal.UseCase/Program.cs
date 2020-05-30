using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Client.Core.Proposal.UseCase
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var log = LoggerFactory.Create(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            }).CreateLogger("Application");

            try
            {
                await Host
                    .CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webHostBuilder =>
                    {
                        webHostBuilder
                            .UseContentRoot(Directory.GetCurrentDirectory())
                            .UseStartup<Startup>();
                    })
                    .ConfigureAppConfiguration((context, config) => InitializeConfiguration(config, args))
                    .ConfigureLogging((context, logging) =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                        logging.AddConfiguration(context.Configuration);
                    })
                    .Build()
                    .RunAsync();
                log.LogInformation("Service started");
                return 0;
            }
            catch (Exception ex)
            {
                log.LogCritical(ex, "Service host terminated unexpectedly.");
                return -1;
            }
        }

        private static void InitializeConfiguration(IConfigurationBuilder builder, string[] args)
        {
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (args != null)
            {
                builder.AddCommandLine(args);
            }
        }
    }
}
