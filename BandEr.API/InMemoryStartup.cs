using BandEr.DAL;
using BandEr.DAL.Entity;
using BandEr.DAL.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BandEr.API
{
    public class InMemoryStartup : Startup
    {
        public InMemoryStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void SetupDbContext(IServiceCollection services)
        {
            services.AddDbContext<BandErDbContext>(builder =>
              builder.UseInMemoryDatabase("BandEr"));
        }

        protected override void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var log = scope.ServiceProvider.GetRequiredService<ILogger<InMemoryStartup>>();
                var context = scope.ServiceProvider.GetRequiredService<BandErDbContext>();
                var um = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                context.SeedAsync(um).Wait();
            }
        }
    }
}
