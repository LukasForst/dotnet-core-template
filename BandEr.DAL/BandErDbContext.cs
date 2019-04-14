using BandEr.DAL.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BandEr.DAL
{
    public class BandErDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public DbSet<ValueEntity> Values { get; set; }
        public BandErDbContext(DbContextOptions<BandErDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ValueEntity>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.Values);
        }
    }
}
