using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Smalec.Api.Abstraction;
using Smalec.Api.Models;

namespace Smalec.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] AuthenticationRequest model)
        {
            model.IpAddress = GetIpAddress();
            var response = await _authService.SignIn(model);

            if (response == null)
                return Unauthorized();

            SetTokenCookie(response.RefreshToken.Token);

            return Ok(new
            {
                token = response.Token
            });
        }

        [HttpGet, Authorize]
        [Authorize(Policy="ActiveToken")]
        public async Task<IActionResult> SignOut()
        {
            var jwt = Request.Headers[HeaderNames.Authorization];
            var refreshToken = Request.Cookies["refreshToken"];
            await _authService.SignOut(jwt, refreshToken);

            return Ok();
        }

        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _authService.RefreshToken(refreshToken, GetIpAddress());

            if (response == null)
                return Unauthorized();

            SetTokenCookie(response.RefreshToken.Token);

            return Ok(new
            {
                token = response.Token
            });
        }

        private string GetIpAddress()
        {
            // skonfigurować serwer też trzeba
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                IsEssential = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
