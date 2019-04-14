using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using BandEr.API.AuthRequirements;

namespace BandEr.API.AuthHandlers
{
    public class ValidApiUserHandler : AuthorizationHandler<ValidApiUseRequirement>
    {
        private readonly ILogger _logger;
        public ValidApiUserHandler(ILogger<ValidApiUserHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidApiUseRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;            
        }
    }
}
