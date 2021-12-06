using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Services.Abstractions.Collection;
using FitnessApp.Serializer.JsonSerializer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace FitnessApp.ApiGateway.Services.Food
{
    public class FoodService<Model, CollectionItem> 
        : CollectionService<Model, CollectionItem>, 
        IFoodService<Model, CollectionItem>
    {
        public FoodService
        (
            IHttpClientFactory httpClientFactory,
            IDistributedCache distributedCache,
            IOptions<AuthenticationSettings> authenticationSettings,
            IOptionsMonitor<CollectionApiClientSettings> optionsMonitor,
            IJsonSerializer serializer,
            ILoggerFactory loggerFactory
        )
            : base("Food", httpClientFactory, distributedCache, authenticationSettings, optionsMonitor, serializer, loggerFactory)
        {
        }
    }
}