using System.Threading.Tasks;
using Smalec.Api.Models;

namespace Smalec.Api.Abstraction
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> SignIn(AuthenticationRequest model);

        Task SignOut(string token, string refreshToken);

        Task<AuthenticationResponse> RefreshToken(string token, string ipAddress);
    }
}