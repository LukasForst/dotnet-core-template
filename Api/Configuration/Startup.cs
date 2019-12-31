using Common.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotnetApp.Configuration
{
    public sealed partial class Startup
    {
        private readonly IConfiguration configuration;

        private readonly IWebHostEnvironment environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // ReSharper disable once UnusedMember.Global
        public void ConfigureServices(IServiceCollection services)
        {
            // unfortunately it is not possible to obtain logger factory for some reason 
            // for that reason, it is not possible to log inside this configuration method

            services.AddControllers();

            ConfigureApplicationJsonSections(services);

            ConfigureDatabase(services);

            var appSettingsSection = configuration.GetSection("AppSettings");
            ConfigureAuthServices(services, new AppSettings {Secret = appSettingsSection["Secret"]});

            CreateDiContainer().Register(services);

            ConfigureSwagger(services);
        }

        private DependencyInjectionConfiguration CreateDiContainer()
        {
            return new DependencyInjectionConfiguration(environment.EnvironmentName);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                logger.LogDebug("Development mode. Using detailed exception page.");
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseOpenApi();
            app.UseSwaggerUi3();

            SeedDatabase(app);
        }
    }
}