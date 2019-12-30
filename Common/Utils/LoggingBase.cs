using Microsoft.Extensions.Logging;

namespace Common.Utils
{
    /// <summary>
    ///     Base class which contains logger.
    /// </summary>
    public abstract class LoggingBase<T>
    {
        /// <summary>
        ///     Logger instance for the specific class.
        /// </summary>
        protected readonly ILogger<T> Logger;

        protected LoggingBase(ILogger<T> logger)
        {
            Logger = logger;
        }
    }
}