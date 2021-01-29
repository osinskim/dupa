using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neo4j.Driver;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Shared.ApiUtils.Extensions;
using Smalec.Lib.Shared.Helpers;
using Smalec.Lib.Shared.Services;
using Smalec.Service.Friendship.Abstraction;
using Smalec.Service.Friendship.Repositories;
using Smalec.Service.Friendship.Utils;
using System;
using System.Reflection;

namespace Smalec.Service.Friendship
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

            AppSettings appSettings = new AppSettings();
            Configuration.GetSection("AppSettings").Bind(appSettings);
            services.AddSingleton<AppSettingsBase>(appSettings);

            services.AddSingleton(GraphDatabase.Driver(
                appSettings.NeoServer,
                AuthTokens.Basic(
                    appSettings.NeoUsername,
                    appSettings.NeoPassword)));

            services.AddDistributedRedisCache(options => {
                options.Configuration = appSettings.RedisAddress;
                options.InstanceName = "";
            });

            services.AddSingleton<ITokensStore, DistributedTokensStore>();
            services.AddSingleton<IServiceProvider>(sp => sp);

            services.AddCustomAuthentication(appSettings);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddConsul(Configuration);

            services.AddScoped<IFriendshipRepository, FriendshipRepository>();
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
