using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Services.Abstractions.Collection;
using FitnessApp.Serializer.JsonSerializer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace FitnessApp.ApiGateway.Services.Exercises
{
    public class ExercisesService<Model, CollectionItem>
        : CollectionService<Model, CollectionItem>,
        IExercisesService<Model, CollectionItem>
    {
        public ExercisesService
        (
            IHttpClientFactory httpClientFactory,
            IDistributedCache distributedCache,
            IOptions<AuthenticationSettings> authenticationSettings,
            IOptionsMonitor<CollectionApiClientSettings> optionsMonitor,
            IJsonSerializer serializer,
            ILoggerFactory loggerFactory
        )
            : base("Exercises", httpClientFactory, distributedCache, authenticationSettings, optionsMonitor, serializer, loggerFactory)
        {
        }
    }
}
