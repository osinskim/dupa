using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using System.Reflection;
using Smalec.Lib.Shared.Helpers;
using Smalec.Service.UserManagement.Abstraction;
using Smalec.Service.UserManagement.Repositories;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Shared.ApiUtils.Extensions;
using Smalec.Service.UserManagement.Validators;
using Smalec.Lib.Shared.Services;
using Smalec.Lib.Shared.Services;
using Smalec.Lib.Shared.Abstraction.Interfaces;

namespace Smalec.Service.UserManagement
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
            services.AddSingleton<AppSettingsBase>(appSettings);

            services.AddScoped<IUserManagementRepository, UserManagementRepository>();
            services.AddTransient<IValidatorsFactory, ValidatorsFactory>();
            services.AddSingleton<ICommonRabbitService, CommonRabbitService>();
            
            services.AddDistributedRedisCache(options => {  
                options.Configuration = appSettings.RedisAddress;  
                options.InstanceName = "";  
            });
            services.AddSingleton<ITokensStore, DistributedTokensStore>();

            services.AddCustomAuthentication(appSettings);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddConsul(Configuration);
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
