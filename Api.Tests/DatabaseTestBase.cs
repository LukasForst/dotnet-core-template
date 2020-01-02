using System;
using System.Collections.Generic;
using Common.Configuration;
using DataAccess.Context;
using DotnetApp.Configuration;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace Api.Tests
{
    /// <summary>
    ///     Test base when the test fixture uses database context.
    /// </summary>
    public abstract class DatabaseTestBase
    {
        /// <summary>
        ///     Collection of built services.
        /// </summary>
        protected virtual IServiceProvider Services => BuildDi(GetServiceCollection());

        /// <summary>
        ///     DbContext instance.
        /// </summary>
        protected virtual AppDbContext DbContext => GetService<AppDbContext>();

        /// <summary>
        ///     Returns instance of the requested service of type T.
        /// </summary>
        protected T GetService<T>()
        {
            return Services.GetService<T>();
        }

        /// <summary>
        ///     Seed method for the database - it is not possible to call <see cref="GetService{T}" /> method during the seed because the
        ///     services are not built in the time when <see cref="SeedDatabase" /> is being executed.
        /// </summary>
        protected virtual void SeedDatabase(AppDbContext context)
        {
        }

        private void AuthServices(IServiceCollection services)
        {
            var authSettings = new AppSettings
            {
                Secret = "uf7NQUolmdgu080401wGT2c6S6ep6Wnk"
            };

            services.AddScoped(provider => Options.Create(authSettings));

            services.AddScoped(provider => Options.Create(new HashingOptions
            {
                Iterations = 1000
            }));

            services.AddScoped(p => GetDataProtectorProviderMock().Object);
        }

        private Mock<IDataProtectionProvider> GetDataProtectorProviderMock()
        {
            var moq = new Mock<IDataProtectionProvider>();
            moq.Setup(x => x.CreateProtector(It.IsAny<string>()))
                .Returns((string x) => GetDataProtectorMock().Object);
            moq.Setup(x => x.CreateProtector(It.IsAny<string>(), It.IsAny<string[]>()))
                .Returns((string x, string[] y) => GetDataProtectorMock().Object);
            moq.Setup(x => x.CreateProtector(It.IsAny<IEnumerable<string>>()))
                .Returns((IEnumerable<string> x) => GetDataProtectorMock().Object);
            return moq;
        }

        private Mock<IDataProtector> GetDataProtectorMock()
        {
            var moq = new Mock<IDataProtector>();
            moq.Setup(x => x.Protect(It.IsAny<byte[]>()))
                .Returns((byte[] x) => x);
            moq.Setup(x => x.Unprotect(It.IsAny<byte[]>()))
                .Returns((byte[] x) => x);
            return moq;
        }

        private IServiceCollection GetServiceCollection()
        {
            // configure in memory database for the tests
            var services = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .AddLogging();

            services.AddScoped(p =>
            {
                // create context
                var context = new AppDbContext(CreateNewContextOptions());
                SeedDatabase(context);
                return context;
            });

            AuthServices(services);
            return services;
        }

        private DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                // connect EF to in memory db
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("main-db-scheme")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        private IServiceProvider BuildDi(IServiceCollection services)
        {
            var diContainer = new DependencyInjectionConfiguration("Test");
            diContainer.Register(services);

            return services.BuildServiceProvider();
        }
    }
}