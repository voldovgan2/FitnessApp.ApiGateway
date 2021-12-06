using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Services.Abstractions.Base;
using FitnessApp.Serializer.JsonSerializer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace FitnessApp.ApiGateway.Services.Settings
{
    public class SettingsService<Model> : GenericService<Model>, ISettingsService<Model>
    {
        public SettingsService
        (
            IHttpClientFactory httpClientFactory,
            IDistributedCache distributedCache,
            IOptions<AuthenticationSettings> authenticationSettings,
            IOptionsMonitor<ApiClientSettings> optionsMonitor,
            IJsonSerializer serializer,
            ILoggerFactory loggerFactory
        )
            : base("Settings", httpClientFactory, distributedCache, authenticationSettings, optionsMonitor, serializer, loggerFactory)
        {
        }
    }
}