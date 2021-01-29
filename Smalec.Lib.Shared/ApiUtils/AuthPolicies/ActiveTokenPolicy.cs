using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Smalec.Lib.Shared.Abstraction.Interfaces;

namespace Smalec.Lib.Shared.ApiUtils.AuthPolicies
{
    public class ActiveTokenPolicy : AuthorizationHandler<ActiveTokenRequirement>
    {
        private readonly ITokensStore _tokensStore;
        private readonly IHttpContextAccessor _httpContext;
        
        public ActiveTokenPolicy(ITokensStore tokensStore, IHttpContextAccessor httpContextAccessor)
        {
            _tokensStore = tokensStore;
            _httpContext = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveTokenRequirement requirement)
        {
            var token = _httpContext.HttpContext.Request.Headers[HeaderNames.Authorization];
            var tokenExists = await _tokensStore.IsTokenActive(token);

            if(tokenExists)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}