using Api.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    /// <summary>
    ///     Dependency injection container settings.
    /// </summary>
    public class DependencyInjectionConfiguration
    {
        /// <summary>
        ///     Name of the environment the application runs in.
        /// </summary>
        private readonly string environmentName;

        public DependencyInjectionConfiguration(string environmentName)
        {
            this.environmentName = environmentName;
        }

        /// <summary>
        ///     Configure DI container.
        /// </summary>
        public void Register(IServiceCollection services)
        {
            ConfigureSingletons(services);
            ConfigureScoped(services);
        }

        /// <summary>
        ///     Method which should register singletons.
        /// </summary>
        private void ConfigureSingletons(IServiceCollection services)
        {
            services.AddSingleton<EncodingService>();
        }

        /// <summary>
        ///     Method for the scoped dependencies. Application DB Context is scoped so everything dependent on this bean,
        ///     should be scoped as well.
        /// </summary>
        private void ConfigureScoped(IServiceCollection services)
        {
            services.AddScoped<AuthenticationService>();
            services.AddScoped<PrincipalService>();
        }
    }
}