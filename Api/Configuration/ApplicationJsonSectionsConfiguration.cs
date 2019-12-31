using Common.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetApp.Configuration
{
    /// <summary>
    ///     Partial part which is used for the registering JSON dtos settings.
    /// </summary>
    public sealed partial class Startup
    {
        /// <summary>
        ///     Configure DTOs for the static JSON settings.
        /// </summary>
        private void ConfigureApplicationJsonSections(IServiceCollection services)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");

            services.Configure<AppSettings>(appSettingsSection);

            services.Configure<HashingOptions>(appSettingsSection.GetSection("PasswordHashing"));
        }
    }
}