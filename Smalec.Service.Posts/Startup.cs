using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smalec.Service.Posts.Abstraction;
using Smalec.Service.Posts.Repositories;
using Smalec.Lib.Shared.Services;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Shared.ApiUtils.Extensions;
using System;
using Smalec.Service.Posts.Strategies.GetPosts;
using Smalec.Service.Posts.Builders.Posts;
using Smalec.Lib.Shared.Helpers;

namespace Smalec.Service.Posts
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
            services.AddSingleton<ICommonRabbitService, CommonRabbitService>();
            services.AddSingleton<IServiceProvider>(sp => sp);
            services.AddDistributedRedisCache(options => {  
                options.Configuration = appSettings.RedisAddress;  
                options.InstanceName = "";  
            });
            services.AddSingleton<ITokensStore, DistributedTokensStore>();
            services.AddScoped<ISocialRepository, SocialRepository>();
            services.AddCustomAuthentication(appSettings);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddConsul(Configuration);
            services.AddScoped<IGetPostsStrategy, MainpagePosts>();
            services.AddScoped<IGetPostsStrategy, MyPosts>();
            services.AddScoped<IGetPostsStrategy, OtherUserPosts>();
            services.AddScoped<IGetPostsStrategy, MyNewlyAddedPost>();
            services.AddScoped<IPostsBuilder, PostsBuilder>();
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
