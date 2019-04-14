using System.Text;
using BandEr.Infrastructure.Auth.Interfaces;

namespace BandEr.Infrastructure.Auth.Extensions
{
    public static class TenantProviderExtensions
    {
        public static string Log(this ITenantProvider provider)
        {
            var builder = new StringBuilder();
            if (provider.IsApiKeyAccess())
                builder.Append(" Api key access.");
            if (provider.IsUserAccess())
                builder.Append(" User access.");
            return builder.ToString();
        }
    }
}
