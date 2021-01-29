using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Shared.ApiUtils.Clients;
using Smalec.Lib.Shared.ApiUtils.Extensions;
using Smalec.Lib.Shared.Helpers;
using Smalec.Lib.Shared.Services;
using System.Reflection;

namespace Smalec.ServiceFacade.Social
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
            services.AddControllers().AddNewtonsoftJson();
            services.AddHttpContextAccessor();

            AppSettingsBase appSettings = new AppSettingsBase();
            Configuration.GetSection("AppSettings").Bind(appSettings);
            services.AddSingleton(appSettings);

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddDistributedRedisCache(options => {
                options.Configuration = appSettings.RedisAddress;
                options.InstanceName = "";
            });
            services.AddSingleton<ITokensStore, DistributedTokensStore>();

            services.AddCustomAuthentication(appSettings);

            services.AddConsul(Configuration);
            services.AddHttpClient<IConsulHttpClient, ConsulHttpClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
