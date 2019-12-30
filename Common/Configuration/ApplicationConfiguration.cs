using System.IO;

namespace Common.Configuration
{
    /// <summary>
    ///     Helper functions for the common configurations.
    /// </summary>
    public static class ApplicationConfiguration
    {
        /// <summary>
        ///     Name of the file where the common shared config is stored.
        /// </summary>
        private const string SharedSettingsFile = "SharedSettings.json";

        /// <summary>
        ///     Returns paths for the shared config file.
        /// </summary>
        public static SharedSettingsPaths GetSharedSettingsPath(string? contentRootPath = null)
        {
            var root = contentRootPath ?? Directory.GetCurrentDirectory();
            return new SharedSettingsPaths(SharedSettingsFile, Path.Combine(root, "..", "Configuration", SharedSettingsFile));
        }
    }

    /// <summary>
    ///     DTO which contains paths to the shared config file.
    /// </summary>
    public class SharedSettingsPaths
    {
        public SharedSettingsPaths(string publishedPath, string developmentPath)
        {
            PublishedPath = publishedPath;
            DevelopmentPath = developmentPath;
        }

        /// <summary>
        ///     Path for the file when the application is published -> therefore the file is compiled/copied in the same directory as the app is running.
        /// </summary>
        public string PublishedPath { get; }

        /// <summary>
        ///     Path that should be used when the application is being run via dotnet run command, because then the app looks for config giles in the real filesystem.
        /// </summary>
        public string DevelopmentPath { get; }

        /// <summary>
        ///     Destructor for both paths.
        /// </summary>
        public void Deconstruct(out string publishedPath, out string developmentPath)
        {
            publishedPath = PublishedPath;
            developmentPath = DevelopmentPath;
        }
    }
}