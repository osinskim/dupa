using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Smalec.Api.Abstraction;
using Smalec.Lib.Shared.ApiUtils.Extensions;
using Smalec.Api.Services;
using Smalec.Lib.Shared.Services;
using Smalec.Lib.Shared.Helpers;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Ocelot.Provider.Consul;

namespace Smalec.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers().AddNewtonsoftJson();
            services.AddHttpContextAccessor();

            AppSettingsBase appSettings = new AppSettingsBase();
            Configuration.GetSection("AppSettings").Bind(appSettings);
            services.AddSingleton<AppSettingsBase>(appSettings);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = appSettings.RedisAddress;
                options.InstanceName = "";
            });

            services.AddSingleton<ITokensStore, DistributedTokensStore>();
            services.AddCustomAuthentication(appSettings);
            services.AddOcelot().AddConsul();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseHsts();
            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(x => x
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOcelot().Wait();
        }
    }
}
