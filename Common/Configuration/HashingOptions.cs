namespace Common.Configuration
{
    /// <summary>
    ///     PasswordHashing section.
    /// </summary>
    public sealed class HashingOptions
    {
        /// <summary>
        ///     Number of iteration an hashing algorithm should use when generating JWT.
        /// </summary>
        public int Iterations { get; set; } = 10000;
    }
}