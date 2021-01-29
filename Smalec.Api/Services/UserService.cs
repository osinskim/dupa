using System.Threading.Tasks;
using Smalec.Api.Abstraction;
using Smalec.Api.Models;
using Smalec.Lib.Shared.Helpers;

namespace Smalec.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AreCredentialsValid(AuthenticationRequest userCredentials)
        {
            if (string.IsNullOrEmpty(userCredentials.UserName) || string.IsNullOrEmpty(userCredentials.Password))
                return false;
            
            var passwordHash = await _userRepository.GetUserPassword(userCredentials.UserName);
            if (string.IsNullOrEmpty(passwordHash))
                return false;
            
            return string.Equals(passwordHash, PasswordHelper.GenerateHash(userCredentials.Password));
        }

        public async Task<string> GetUserUuidByEmail(string userName) 
            => await _userRepository.GetUserUuidByEmail(userName);
    }
}