using Common.Utils;
using DataAccess.Context;
using Microsoft.Extensions.Logging;

namespace Api.Services
{
    /// <summary>
    ///     Base class for the services which contains <see cref="DbContext" />.
    /// </summary>
    public abstract class ServiceBase<T> : LoggingBase<T>
    {
        /// <summary>
        ///     Instance of the context.
        /// </summary>
        protected readonly AppDbContext DbContext;

        protected ServiceBase(ILogger<T> logger, AppDbContext dbContext) : base(logger)
        {
            DbContext = dbContext;
        }
    }
}