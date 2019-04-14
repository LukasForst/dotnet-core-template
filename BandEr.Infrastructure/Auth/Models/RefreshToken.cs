using System;

namespace BandEr.Infrastructure.Auth.Models
{
    public class RefreshToken<TKey, TToken>
    {
        public TToken Token { get; private set; }
        public DateTime Expires { get; private set; }
        public TKey UserId { get; private set; }
        public bool Active => DateTime.UtcNow <= Expires;

        public RefreshToken(TToken token, DateTime expires, TKey userId)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
        }
    }
}