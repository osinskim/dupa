using System.Threading.Tasks;
using Smalec.Api.Models;

namespace Smalec.Api.Abstraction
{
    public interface IUserService
    {
        Task<bool> AreCredentialsValid(AuthenticationRequest model);
        Task<string> GetUserUuidByEmail(string userName);
    }
}