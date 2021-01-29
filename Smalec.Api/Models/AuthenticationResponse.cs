using Smalec.Lib.Shared.Helpers;

namespace Smalec.Api.Models
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }

        public RefreshToken RefreshToken { get; set; }
    }
}