using Common.Utils;
using DataAccess.Context;
using Microsoft.Extensions.Logging;

namespace DotnetApp.Services
{
    public abstract class ServiceBase<T> : LoggingBase<T>
    {
        protected readonly AppDbContext DbContext;

        protected ServiceBase(ILogger<T> logger, AppDbContext dbContext) : base(logger)
        {
            DbContext = dbContext;
        }
    }
}