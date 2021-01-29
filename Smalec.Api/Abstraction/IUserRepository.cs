using System.Threading.Tasks;

namespace Smalec.Api.Abstraction
{
    public interface IUserRepository
    {
        Task<string> GetUserPassword(string username);
        Task<string> GetUserUuidByEmail(string userName);
    }
}