using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DotnetApp.DTO.Auth
{
    /// <summary>
    ///     DTO for user login.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] // because this is set by middleware
    public class LoginRequest
    {
        /// <summary>
        ///     Username.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        ///     User's password.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}