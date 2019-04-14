using BandEr.Infrastructure.Auth.Extensions;
using BandEr.Infrastructure.Auth.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BandEr.Infrastructure.Auth.Services
{
    public class TokenTenantProvider : ITenantProvider
    {
        private readonly HttpContext _httpContext;

        public TokenTenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public string GetUserId()
        {
            return _httpContext
                ?.User
                ?.GetUserId();
        }

        public string GetUsername()
        {
            return _httpContext
                ?.User
                ?.Identity
                ?.Name;
        }

        public bool HasRole(string role)
        {
            return _httpContext
                ?.User
                ?.IsInRole(role)
                ?? false;
        }

        public bool IsApiKeyAccess()
        {
            return _httpContext
                ?.User
                .IsApiKeyAccess()
                ?? false;
        }

        public bool IsUserAccess()
        {
            return _httpContext
                ?.User
                .IsUserAccess()
                ?? false;
        }
    }
}