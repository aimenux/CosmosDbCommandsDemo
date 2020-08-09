using System;
using System.Diagnostics;
using System.Linq;
using App.Commands;
using App.Options;
using Lib;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;

namespace App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();

            var commands = host.Services
                .GetServices<ICommand>()
                .OfType<CommandLineApplication>()
                .ToList();

            var app = new CommandLineApplication
            {
                Name = "CosmosDbCommandsDemo",
                Description = "Run commands on CosmosDb container"
            };

            app.Commands.AddRange(commands);

            app.HelpOption("-?|-h|--help");

            app.OnExecute(() => {
                app.ShowHelp();
                return 0;
            });

            app.Execute(args);

            PressAnyKeyToExit();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var environment = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "DEV";
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
                config.AddCommandLine(args);
            })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.AddSerilogLogger();
                loggingBuilder.AddDemoLogger();
                loggingBuilder.AddConsole();
            })
            .ConfigureServices((hostingContext, services) =>
            {
                services.AddSingleton<ToCommandOption>();
                services.AddSingleton<FromCommandOption>();
                services.AddSingleton<TimeToLiveCommandOption>();
                services.AddSingleton<ICommand, InitializeCommand>();
                services.AddSingleton<ICommand, TimeToLiveCommand>();
                services.AddSingleton<ICosmosDbClient<CosmosDbDocument>>(_ =>
                {
                    var endpointUrl = hostingContext.Configuration.GetValue<string>("CosmosDb:Url");
                    var authKey = hostingContext.Configuration.GetValue<string>("CosmosDb:Key");
                    var databaseName = hostingContext.Configuration.GetValue<string>("CosmosDb:DatabaseName");
                    var containerName = hostingContext.Configuration.GetValue<string>("CosmosDb:ContainerName");
                    return new CosmosDbClient<CosmosDbDocument>(endpointUrl, authKey, databaseName, containerName);
                });
            });

        private static void AddSerilogLogger(this ILoggingBuilder loggingBuilder)
        {
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithMachineName()
                .Enrich.WithExceptionDetails()
                .Enrich.WithEnvironmentUserName()
                .CreateLogger();

            loggingBuilder.AddSerilog(Log.Logger);
        }

        private static void AddDemoLogger(this ILoggingBuilder loggingBuilder)
        {
            var services = loggingBuilder.Services;
            services.AddSingleton(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                return loggerFactory.CreateLogger("CosmosDbCommandsDemo");
            });
        }

        private static void PressAnyKeyToExit()
        {
            if (!Debugger.IsAttached) return;
            Console.WriteLine("Press any key to exit !");
            Console.ReadKey();
        }
    }
}
