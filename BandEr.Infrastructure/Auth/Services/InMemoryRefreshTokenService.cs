using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BandEr.Infrastructure.Auth.Interfaces;
using BandEr.Infrastructure.Auth.Models;

namespace BandEr.Infrastructure.Auth.Services
{
    public sealed class InMemoryRefreshTokenService<TKey, TToken> : IRefreshTokenService<TKey, TToken>
    {
        private readonly Dictionary<TToken, RefreshToken<TKey, TToken>> _tokens = new Dictionary<TToken, RefreshToken<TKey, TToken>>();

        public Task<bool> HasValidRefreshTokenAsync(TToken refreshToken, TKey userId)
        {
            return _tokens.TryGetValue(refreshToken, out var token) ?
                Task.FromResult(token.Active && token.UserId.Equals(userId))
                : Task.FromResult(false);
        }

        public Task AddRefreshTokenAsync(TToken token, TKey userId, double daysToExpire = 5)
        {
            _tokens[token] = new RefreshToken<TKey, TToken>(token, DateTime.UtcNow.AddDays(daysToExpire), userId);
            return Task.CompletedTask;
        }

        public Task<bool> RemoveRefreshTokenAsync(TToken refreshToken)
        {
            return Task.FromResult(_tokens.Remove(refreshToken));
        }
    }
}