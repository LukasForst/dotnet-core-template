using System;
using System.Threading.Tasks;
using BandEr.DAL;
using BandEr.DAL.Entity;
using BandEr.Infrastructure.Auth.Interfaces;
using BandEr.Infrastructure.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BandEr.API.Controllers
{
    public class SignIn
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthController : ApiControllerBase
    {
        private readonly BandErDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenFactory _tokenFactory;
        private readonly IJwtFactory _jwtFactory;
        private readonly IRefreshTokenService<int, string> _refreshTokenService;

        public AuthController(BandErDbContext context, UserManager<AppUser> userManager, ITokenFactory tokenFactory, IJwtFactory jwtFactory, IRefreshTokenService<int, string> refreshTokenService)
        {
            _context = context;
            _userManager = userManager;
            _tokenFactory = tokenFactory;
            _jwtFactory = jwtFactory;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<LoginResponse> PostAsync([FromBody] SignIn request)
        {
            var user = await _context.Users
               .FirstOrDefaultAsync(x => x.UserName == request.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                // generate refresh token
                var refreshToken = _tokenFactory.GenerateToken();
                // generate access token
                var roles = await _userManager.GetRolesAsync(user);
                var accessToken = await _jwtFactory.GenerateEncodedToken(user.Id.ToString(), user.UserName, roles);
                await _refreshTokenService.AddRefreshTokenAsync(refreshToken, user.Id);
                return new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            throw new UnauthorizedAccessException($"Wrong username or password.");
        }
    }
}
