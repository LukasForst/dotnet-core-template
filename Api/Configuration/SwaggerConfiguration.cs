using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace DotnetApp.Configuration
{
    /// <summary>
    ///     Swagger configuration.
    /// </summary>
    public sealed partial class Startup
    {
        /// <summary>
        ///     Configure swagger settings.
        /// </summary>
        private void ConfigureSwagger(IServiceCollection services)
        {
            // TODO configure enum to string converter
            services.AddOpenApiDocument(config =>
            {
                config.Title = "Example .NET Core Base Application";
                config.Version = "v1";
                config.Description = "An example .NET Core application";

                config.AddSecurity("JWT", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });
                config.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT"));
            });
        }
    }
}