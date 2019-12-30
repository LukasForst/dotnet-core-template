using Common.Configuration;
using DotnetApp.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotnetApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    // add console logs
                    logging.AddConsole();
                })
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    // obtain current environment state
                    var env = builderContext.HostingEnvironment;
                    // get shared application settings
                    var (publishedPath, developmentPath) = ApplicationConfiguration.GetSharedSettingsPath(env.ContentRootPath);

                    config
                        .AddJsonFile(publishedPath, true) // When compiled and deployed
                        .AddJsonFile(developmentPath, false) // When running using dotnet run
                        .AddJsonFile("appsettings.json", false)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                        .AddEnvironmentVariables()
                        .Build();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}