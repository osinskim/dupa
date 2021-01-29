using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Shared.Helpers;

namespace Smalec.Lib.Shared.Services
{
    public class DistributedTokensStore : ITokensStore
    {
        private readonly IDistributedCache _cache;

        public DistributedTokensStore(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task AddToken(string token)
        {
            await _cache.SetStringAsync(BuildTokenEntry(token), " ",
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
        }

        public async Task<bool> IsTokenActive(string token)
        {
            var tokenFromCache = await _cache.GetStringAsync(BuildTokenEntry(token));
            return tokenFromCache != null;
        }

        public async Task RemoveToken(string token)
        {
            await _cache.RemoveAsync(BuildTokenEntry(token));
        }

        public async Task AddRefreshToken(RefreshToken token)
        {
            await _cache.SetAsync(
                token.Token,
                SerializeRefreshToken(token),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                }
            );
        }

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            var serializedToken = await _cache.GetAsync(token);
            if(serializedToken == null) return null;

            return JsonConvert.DeserializeObject<RefreshToken>(Encoding.ASCII.GetString(serializedToken));
        }

        public async Task RemoveRefreshToken(string token)
        {
            await _cache.RemoveAsync(token);
        }

        private static string BuildTokenEntry(string token)
        {
            return $"tokens:{token}:activated";
        }

        private byte[] SerializeRefreshToken(RefreshToken token)
        {
            var tokenString = JsonConvert.SerializeObject(token);

            return Encoding.ASCII.GetBytes(tokenString);  
        }
    }
}