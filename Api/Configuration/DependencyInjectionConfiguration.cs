using DotnetApp.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetApp.Configuration
{
    /// <summary>
    ///     Dependency injection container settings.
    /// </summary>
    public class DependencyInjectionContainer
    {
        private readonly string environmentName;

        public DependencyInjectionContainer(string environmentName)
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
            services.AddScoped<EncodingService>();
            services.AddScoped<PrincipalService>();
        }
    }
}