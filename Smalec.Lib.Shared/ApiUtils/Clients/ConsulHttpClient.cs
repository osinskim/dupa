using Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Smalec.Lib.Shared.ApiUtils.Clients
{
    public class ConsulHttpClient : IConsulHttpClient
    {
        private readonly HttpClient _http;
        private readonly IConsulClient _consul;
        private readonly IHttpContextAccessor _currentContext;

        public ConsulHttpClient(HttpClient http, IConsulClient consul, IHttpContextAccessor currentContext)
        {
            _http = http;
            _consul = consul;
            _currentContext = currentContext;
        }

        public async Task<string> Post(string serviceName, string resourcePath, HttpContent content)
        {
            AddJwtToken();
            var uri = await GetRequestUri(serviceName, resourcePath, string.Empty);
            var response = await _http.PostAsync(uri, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("kuuurwa znowu sie zjebało");

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Get(string serviceName, string resourcePath, string urlQuery = "")
        {
            AddJwtToken();
            var uri = await GetRequestUri(serviceName, resourcePath, urlQuery);
            var response = await _http.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
                throw new Exception("kuuurwa znowu sie zjebało ten get tym razem");

            return await response.Content.ReadAsStringAsync();
        }

        private void AddJwtToken()
        {
            var tokenWithSchema = _currentContext.HttpContext.Request.Headers[HeaderNames.Authorization].ToString();

            if (string.IsNullOrEmpty(tokenWithSchema))
                return;

            var token = tokenWithSchema.Split().Skip(1).FirstOrDefault();

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<Uri> GetRequestUri(string serviceName, string resourcePath, string urlQuery)
        {
            var allRegisteredServices = await _consul.Agent.Services();

            var servicesAvailableForCurrentRequest = allRegisteredServices.Response
                ?.Where(x => string.Equals(x.Value.Service, serviceName, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value)
                .ToList();

            var service = GetRandomInstance(servicesAvailableForCurrentRequest);

            var uriBuilder = new UriBuilder()
            {
                Host = service.Address,
                Port = service.Port,
                Path = resourcePath,
                Query = urlQuery
            };

            return uriBuilder.Uri;
        }

        private AgentService GetRandomInstance(IList<AgentService> services)
        {
            if (!services.Any())
                throw new Exception("zjebało sie kurwa :C");

            var random = new Random();
            return services[random.Next(0, services.Count - 1)];
        }
    }
}
