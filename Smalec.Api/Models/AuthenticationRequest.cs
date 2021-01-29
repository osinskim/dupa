namespace Smalec.Api.Models
{
    public class AuthenticationRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string IpAddress { get; set; }
    }
}