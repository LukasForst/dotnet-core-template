using System.Text;
using Common.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Api.Configuration
{
    /// <summary>
    ///     Authorization and authentication settings.
    /// </summary>
    public sealed partial class Startup
    {
        /// <summary>
        ///     Configure services for te authentication and authorization.
        /// </summary>
        private void ConfigureAuthServices(IServiceCollection services, AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;
                    // basic parameters, for the improved security, validate issuer and audience
                    // in order to do that, one must update AuthenticationService as well
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        // because we do not sing it yea
                        ValidateIssuer = false,
                        // also, we cont set the audience 
                        ValidateAudience = false
                    };
                });
        }
    }
}