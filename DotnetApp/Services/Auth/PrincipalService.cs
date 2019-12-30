using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.Model;
using Microsoft.Extensions.Logging;

namespace DotnetApp.Services.Auth
{
    /// <summary>
    ///     Service which maps user claims to user db object.
    /// </summary>
    public class PrincipalService : ServiceBase<PrincipalService>
    {
        public PrincipalService(AppDbContext dbContext, ILogger<PrincipalService> logger) : base(logger, dbContext)
        {
        }

        /// <summary>
        ///     Returns <see cref="User" /> database object for given principal. Throws exception if the user does not exist or the identity is invalid.
        /// </summary>
        public async Task<User> GetUserForClaims(ClaimsPrincipal principal)
        {
            // this should not happen, but just to be sure and make the compiler happy
            if (!(principal.Identity is ClaimsIdentity identity))
            {
                Logger.LogError("It was not possible to obtain principal identity!");
                throw new ArgumentException("It was not possible to obtain principal identity!");
            }

            // we store userId in the identity Name - see AuthenticationService
            if (!int.TryParse(identity.Name, out var userId))
            {
                Logger.LogError($"It was not possible to parse user id. Identity name was {identity.Name}.");
                throw new ArgumentException("It was not possible to obtain user id from the principal identity!");
            }

            var user = await DbContext.Users.FindAsync(userId) ?? throw new ArgumentException($"It was not possible to find user with id {userId}.");
            return user;
        }
    }
}