using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Smalec.Api.Abstraction;
using Smalec.Api.Models;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Shared.Helpers;

namespace Smalec.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettingsBase _appSettings;
        private readonly IUserService _userService;
        private readonly ITokensStore _tokensStore;

        public AuthenticationService(AppSettingsBase appSettings, IUserService userService, ITokensStore tokensStore)
        {
            _appSettings = appSettings;
            _userService = userService;
            _tokensStore = tokensStore;
        }

        public async Task<AuthenticationResponse> SignIn(AuthenticationRequest request)
        {
            if (!(await _userService.AreCredentialsValid(request)))
                return null;

            var userUuid = await _userService.GetUserUuidByEmail(request.UserName);
            var response = new AuthenticationResponse
            {
                Token = GenerateToken(userUuid),
                RefreshToken = GenerateRefreshToken(request.IpAddress, userUuid)
            };

            await _tokensStore.AddToken("Bearer " + response.Token);
            await _tokensStore.AddRefreshToken(response.RefreshToken);

            return response;
        }

        public async Task<AuthenticationResponse> RefreshToken(string token, string ipAddress)
        {
            if(string.IsNullOrEmpty(token))
            {
                return null;
            }
            
            var tokenObject = await _tokensStore.GetRefreshToken(token);
            if(tokenObject == null || IsRefreshTokenExpired(tokenObject) || !string.Equals(tokenObject.CreatedByIp, ipAddress))
            {
                return null;
            }

            var response = new AuthenticationResponse
            {
                Token = GenerateToken(tokenObject.CreatedByUser),
                RefreshToken = GenerateRefreshToken(ipAddress, tokenObject.CreatedByUser)
            };

            await _tokensStore.AddToken("Bearer " + response.Token);
            await _tokensStore.AddRefreshToken(response.RefreshToken);

            return response;
        }

        public async Task SignOut(string token, string refreshToken)
        {
            await _tokensStore.RemoveToken(token);
            await _tokensStore.RemoveRefreshToken(refreshToken);
        }

        private string GenerateToken(string userUuid)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userUuid)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool IsRefreshTokenExpired(RefreshToken token)
        {
            return (token.Created + TimeSpan.FromDays(7)) <= DateTime.Now;
        }

        private RefreshToken GenerateRefreshToken(string ipAddress, string user)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress,
                    CreatedByUser = user
                };
            }
        }
    }
}