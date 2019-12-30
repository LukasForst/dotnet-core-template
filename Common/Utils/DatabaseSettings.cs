using Microsoft.Extensions.Configuration;

namespace Common.Utils
{
    /// <summary>
    ///     Static helper utils for the database settings.
    /// </summary>
    public static class DatabaseSettings
    {
        /// <summary>
        ///     Gets connection string for the given settings from the static JSON file.
        /// </summary>
        public static string ResolveConnectionString(this IConfiguration configuration, ConnectionString connectionString)
        {
            var connectionStringAsString = connectionString.ToString();
            if (connectionString == ConnectionString.Default) connectionStringAsString = configuration.GetConnectionString(connectionStringAsString);

            return configuration.GetConnectionString(connectionStringAsString);
        }
    }

    /// <summary>
    ///     Connection string type.
    /// </summary>
    public enum ConnectionString
    {
        /// <summary>
        ///     DB running in the Docker, application in the localhost.
        /// </summary>
        Docker,

        /// <summary>
        ///     Both application and the DB are running in the docker in the docker-compose environment.
        /// </summary>
        DockerCompose,

        /// <summary>
        ///     Resolves default connection string as used in the ConnectionString section in the SharredSettings.json.
        /// </summary>
        Default
    }
}