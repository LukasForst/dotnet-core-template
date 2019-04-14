using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;

namespace BandEr.Infrastructure.Auth.Models
{
    public sealed class AccessToken
    {
        public string Token { get; }
        public int ExpiresIn { get; }
        [JsonIgnore]
        public List<Claim> Claims { get; } = new List<Claim>();
        public string Jti { get; }

        [JsonConstructor]
        public AccessToken(string token, int expiresIn)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }

        public AccessToken(string token, int expiresIn, IEnumerable<Claim> claims) : this(token, expiresIn)
        {
            Claims = claims.ToList();
            Jti = Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
        }
    }
}