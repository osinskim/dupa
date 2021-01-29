using Microsoft.AspNetCore.Authorization;

namespace Smalec.Lib.Shared.ApiUtils.AuthPolicies
{
    public class ActiveTokenRequirement: IAuthorizationRequirement
    {}
}