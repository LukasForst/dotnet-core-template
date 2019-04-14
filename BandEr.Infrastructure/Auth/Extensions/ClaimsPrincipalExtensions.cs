using System.Linq;
using System.Security.Claims;
using BandEr.Infrastructure.Auth.Helpers;
using static BandEr.Infrastructure.Auth.Helpers.Constants.Strings;

namespace BandEr.Infrastructure.Auth.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsUserAccess(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.HasClaim(c => c.Type == JwtClaimIdentifiers.Rol && c.Value == Constants.Strings.JwtClaims.UserAccess);
        }
        public static bool IsApiKeyAccess(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.HasClaim(c => c.Type == JwtClaimIdentifiers.Rol && c.Value == Constants.Strings.JwtClaims.ApiKeyAccess);
        }

        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtClaimIdentifiers.Id)?.Value;
        }
    }
}
