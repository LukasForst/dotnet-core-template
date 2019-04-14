using System.Security.Claims;

namespace BandEr.Infrastructure.Auth.Interfaces
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}