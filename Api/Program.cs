using Api.Configuration;
using Common.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api
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
                        .AddJsonFile(developmentPath, true) // When running using dotnet run
                        .AddJsonFile("appsettings.json", false)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args)
                        .Build();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}