using System.Threading.Tasks;
using DataAccess.Model;
using DotnetApp.DTO.Auth;
using DotnetApp.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApp.Controllers
{
    /// <summary>
    ///     Controller for the user interaction.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AuthenticationService authenticationService;
        private readonly EncodingService encodingService;
        private readonly PrincipalService principalService;

        public UsersController(AuthenticationService authenticationService, EncodingService encodingService, PrincipalService principalService)
        {
            this.authenticationService = authenticationService;
            this.encodingService = encodingService;
            this.principalService = principalService;
        }

        /// <summary>
        ///     Login the user. Returns the generated Token.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [Produces("application/json")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest model)
        {
            var token = await authenticationService.Authenticate(model.Username, model.Password);
            if (token == null) return Unauthorized(new {message = "Username or password is incorrect"});

            return Ok(token);
        }

        /// <summary>
        ///     Testing endpoint, which generates hash for the given password.
        /// </summary>
        [HttpPost("hash/{password}")]
        public async Task<string> GenerateHash(string password)
        {
            return await Task.Run(() => encodingService.Encode(password));
        }

        /// <summary>
        ///     Returns database object for the current user, which is logged in.
        /// </summary>
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public Task<User> GetAuthorized()
        {
            return principalService.GetUserForClaims(User);
        }

        /// <summary>
        ///     Testing endpoint which just returns OK when called.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("unauthorized")]
        public Task<string> UnauthorizedAllowed()
        {
            return Task.Run(() => "Ok - not authorized");
        }
    }
}