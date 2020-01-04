using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Common.Configuration;
using Common.Utils;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Context
{
    /// <summary>
    ///     Application DB context.
    /// </summary>
    // because the DB sets must have setters
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public sealed class AppDbContext : DbContext
    {
        /// <summary>
        ///     Empty constructor is required for the database migrations. Properties are injected in the runtime.
        /// </summary>
        // ReSharper disable once UnusedMember.Global -- necessary for the EF
        public AppDbContext()
        {
            Debug.Assert(Database != null, nameof(Database) + " != null");
            Database.SetCommandTimeout(2);
        }


        /// <summary>
        ///     Constructor required by EF Core.
        /// </summary>
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        ///     Example model used in the first migration.
        /// </summary>
        public DbSet<ExampleModel> ExampleModels { get; private set; } = null!;

        /// <summary>
        ///     Application users.
        /// </summary>
        // null! -> https://docs.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types
        public DbSet<User> Users { get; private set; } = null!;

        /// <summary>
        ///     Configuration only for the migrations. Therefore DB settings and connection strings must be stored in the shared config file.
        /// </summary>
        private static IConfiguration CreateConfiguration()
        {
            var (publishedPath, developmentPath) = ApplicationConfiguration.GetSharedSettingsPath();

            return new ConfigurationBuilder()
                .AddJsonFile(publishedPath, true) // When compiled and deployed
                .AddJsonFile(developmentPath, true) // When running using dotnet run
                .AddJsonFile("appsettings.json", true)
                .Build();
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // do not continue if the auto configuration from the EF was used
            if (optionsBuilder.IsConfigured) return;

            // configure on the migrations
            var configuration = CreateConfiguration();
            optionsBuilder.UseSqlServer(configuration.ResolveConnectionString(ConnectionString.Default));
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}