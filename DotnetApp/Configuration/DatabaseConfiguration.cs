using Common.Utils;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetApp.Configuration
{
    /// <summary>
    ///     Database configuration settings.
    /// </summary>
    public sealed partial class Startup
    {
        /// <summary>
        ///     Configure settings for the database.
        /// </summary>
        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(x =>
                // use default connection string
                x.UseSqlServer(Configuration.ResolveConnectionString(ConnectionString.Default)));
        }
    }
}