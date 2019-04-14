using System.Linq;
using System.Threading.Tasks;
using BandEr.DAL.Entity;
using Microsoft.AspNetCore.Identity;

namespace BandEr.DAL.Extensions
{
    public static class BandErDbContextExtensions
    {
        public static async Task SeedAsync(this BandErDbContext context, UserManager<AppUser> userManager)
        {
            if(!context.Users.Any())
            {
                var user = new AppUser
                {                    
                    UserName = "admin@email.com",
                    Email = "admin@email.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "Password12!");
            }
        }        
    }
}
