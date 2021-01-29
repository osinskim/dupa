using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Smalec.Lib.Shared.ApiUtils.AuthPolicies;
using Smalec.Lib.Shared.Helpers;

namespace Smalec.Lib.Shared.ApiUtils.Extensions
{
    public static partial class ApiExtensions
    {
        public static void AddCustomAuthentication(this IServiceCollection services, AppSettingsBase appSettings)
        {
            services.AddSingleton<IAuthorizationHandler, ActiveTokenPolicy>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                defaultAuthorizationPolicyBuilder =  defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

                options.AddPolicy("ActiveToken", policy =>
                    policy.Requirements.Add(new ActiveTokenRequirement()));
                });
        }
    }
}