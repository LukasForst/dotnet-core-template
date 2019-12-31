using System;
using System.Linq;
using System.Security.Cryptography;
using Common.Configuration;
using Microsoft.Extensions.Options;

namespace DotnetApp.Services.Auth
{
    /// <summary>
    ///     Class provides methods for the password hashing and matching the passwords to the given hashes.
    /// </summary>
    public class EncodingService
    {
        private const int SaltSize = 16; // 128 bit 
        private const int KeySize = 32; // 256 bit

        public EncodingService(IOptions<HashingOptions> options)
        {
            Options = options.Value;
        }

        private HashingOptions Options { get; }

        /// <summary>
        ///     Returns salted hash for the given password.
        /// </summary>
        public string Encode(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Options.Iterations, HashAlgorithmName.SHA256);
            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{Options.Iterations}.{salt}.{key}";
        }

        /// <summary>
        ///     Checks, whether given password corresponds to the hash. Returns true iff the password corresponds to the hash, false otherwise.
        /// </summary>
        public bool DoesHashCorrespondToPassword(string password, string hash)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3) throw new FormatException("Unexpected hash format. Should be formatted as `{iterations}.{salt}.{hash}`");

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            using var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256);

            var keyToCheck = algorithm.GetBytes(KeySize);

            return keyToCheck.SequenceEqual(key);
        }
    }
}