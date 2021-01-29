using Consul;
using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smalec.Lib.Shared.Helpers;
using Microsoft.Extensions.Hosting;
using Smalec.Lib.Shared.Services;

namespace Smalec.Lib.Shared.ApiUtils.Extensions
{
    public static partial class ApiExtensions
    {
        public static void AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            var name = Dns.GetHostName();
            var serviceConfig = new ServiceConfig
            {
                ServiceDiscoveryAddress = configuration.GetValue<Uri>("ServiceConfig:serviceDiscoveryAddress"),
                ServiceAddress = new Uri($"http://{name}:80"),
                ServiceName = configuration.GetValue<string>("ServiceConfig:serviceName"),
                ServiceId = configuration.GetValue<string>("ServiceConfig:serviceId") + name
            };
            
            var consulClient = CreateConsulClient(serviceConfig);

            services.AddSingleton(serviceConfig);
            services.AddSingleton<IHostedService, ServiceDiscoveryHostedService>();
            services.AddSingleton<IConsulClient, ConsulClient>(p => consulClient);
        }

        private static ConsulClient CreateConsulClient(ServiceConfig serviceConfig)
        {
            return new ConsulClient(config =>
            {
                config.Address = serviceConfig.ServiceDiscoveryAddress;
            });
        }
    }
}