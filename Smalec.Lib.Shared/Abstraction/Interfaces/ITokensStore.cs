using System.Threading.Tasks;
using Smalec.Lib.Shared.Helpers;

namespace Smalec.Lib.Shared.Abstraction.Interfaces
{
    public interface ITokensStore
    {
        Task<bool> IsTokenActive(string token);

        Task AddToken(string token);

        Task RemoveToken(string token);

        Task AddRefreshToken(RefreshToken token);

        Task RemoveRefreshToken(string token);

        Task<RefreshToken> GetRefreshToken(string token);
    }
}