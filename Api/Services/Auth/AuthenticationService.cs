using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common.Configuration;
using DataAccess.Context;
using DotnetApp.DTO.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DotnetApp.Services.Auth
{
    /// <summary>
    ///     Service used for user authentication.
    /// </summary>
    public class AuthenticationService : ServiceBase<AuthenticationService>
    {
        private readonly AppSettings appSettings;
        private readonly EncodingService encodingService;

        public AuthenticationService(
            EncodingService encodingService,
            IOptions<AppSettings> appSettings,
            AppDbContext dbContext,
            ILogger<AuthenticationService> logger
        ) : base(logger, dbContext)
        {
            this.appSettings = appSettings.Value;
            this.encodingService = encodingService;
        }

        /// <summary>
        ///     Generates token for the given username and password. If the user does not exist or the password is incorrect, null is returned.
        /// </summary>
        public async Task<TokenDto?> Authenticate(string username, string password)
        {
            Logger.LogDebug($"Login request for user - {username}. ");
            var user = await DbContext.Users.SingleOrDefaultAsync(x => x.Username == username);
            // user does not exist or the password is not correct
            if (user == null || !encodingService.DoesHashCorrespondToPassword(password, user.PasswordHash))
            {
                Logger.LogWarning(
                    $"It was not possible to authenticate user with username {username}, because it does not exist or password is incorrect."
                );
                return null;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    // store user id in the Name part of the claims to identify user 
                    // if this is changed in a way, that there will be stored something different,
                    // please update PrincipalService accordingly
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            Logger.LogDebug($"Token generated for {username} with expiration {tokenDescriptor.Expires.Value}.");
            return new TokenDto(token, tokenDescriptor.Expires.Value);
        }
    }
}