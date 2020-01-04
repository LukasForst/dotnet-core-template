using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Api.Configuration
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
            services.AddOpenApiDocument(config =>
            {
                config.Title = "Example .NET Core Base Application";
                config.Version = "v1";
                config.Description = "An example .NET Core application";

                config.SchemaGenerator.Settings.SerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = config.SchemaGenerator.Settings.ActualContractResolver,
                    Converters = new List<JsonConverter> { new StringEnumConverter() }
                };

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