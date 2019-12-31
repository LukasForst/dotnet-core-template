namespace Common.Configuration
{
    /// <summary>
    ///     AppSettings section of the configuration json.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        ///     Secret used for the JWT singing.
        /// </summary>
        public string Secret { get; set; } = "YellowHorseSeemsToBeCorrectSecret";
    }
}