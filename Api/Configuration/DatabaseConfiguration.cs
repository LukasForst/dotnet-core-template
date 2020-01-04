using System.Linq;
using Common.Utils;
using DataAccess.Context;
using DataAccess.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
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
                x.UseSqlServer(ObtainConnectionString()));
        }

        private string ObtainConnectionString()
        {
            var connectionString = configuration.GetValue<string>("connection-string");
            if (connectionString == null)
            {
                var connectionStringOption = configuration.GetValue("db-env", ConnectionString.Default);
                connectionString = configuration.ResolveConnectionString(connectionStringOption);
            }

            return connectionString;
        }


        /// <summary>
        ///     Inserts predefined data into the database.
        /// </summary>
        private void SeedDatabase(IApplicationBuilder app)
        {
            var adminUser = new User(
                "Service",
                "User",
                "admin",
                "1000.PpqwnVxXYtMKIjc7SZ9jdA==.ikYDT+YSJLn9c3Slw4fC61MisaBgYvVaTqcGZVw875c=", // password "hello"
                "Admin"
            );

            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Users.Count(x => x.Username == adminUser.Username) == 0)
            {
                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}