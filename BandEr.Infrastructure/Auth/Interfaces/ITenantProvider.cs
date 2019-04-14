namespace BandEr.Infrastructure.Auth.Interfaces
{
    public interface ITenantProvider
    {
        string GetUserId();
        string GetUsername();        
        bool IsUserAccess();
        bool IsApiKeyAccess();        
        bool HasRole(string role);
    }
}
