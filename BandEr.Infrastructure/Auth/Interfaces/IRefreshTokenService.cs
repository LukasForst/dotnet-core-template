using System.Threading.Tasks;

namespace BandEr.Infrastructure.Auth.Interfaces
{
    public interface IRefreshTokenService<TKey, TToken>
    {
        Task AddRefreshTokenAsync(TToken token, TKey userId, double daysToExpire = 5);
        Task<bool> HasValidRefreshTokenAsync(TToken refreshToken, TKey userId);
        Task<bool> RemoveRefreshTokenAsync(TToken refreshToken);
    }
}