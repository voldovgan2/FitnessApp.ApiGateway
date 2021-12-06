using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.Serializer.JsonSerializer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.Abstractions.Collection
{
    public class CollectionService<Model, CollectionItem> : ICollectionService<Model, CollectionItem>
    {
        private readonly IInternalClient _internalClient;
        private readonly CollectionApiClientSettings _apiClientSettings;

        public CollectionService
        (
            string name,
            IHttpClientFactory httpClientFactory,
            IDistributedCache distributedCache,
            IOptions<AuthenticationSettings> authenticationSettings,
            IOptionsMonitor<CollectionApiClientSettings> optionsMonitor,
            IJsonSerializer serializer,
            ILoggerFactory loggerFactory
        )
        {
            _apiClientSettings = optionsMonitor.Get(name);
            _apiClientSettings.ApiName = name;
            _internalClient = new InternalClient.InternalClient
            (
                httpClientFactory,
                distributedCache,
                authenticationSettings.Value,
                name,
                _apiClientSettings.Scope,
                serializer,
                loggerFactory
            );
        }

        public async Task<Model> GetItemAsync(object model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Get, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                _apiClientSettings.GetItemsMethodName,
                null, 
                null,
                model
            );
            var result = await _internalClient.SendInternalRequest<Model>(request, CancellationToken.None);
            return result;
        }

        public async Task<CollectionItem> AddItemAsync(object model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Put, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                _apiClientSettings.AddItemMethodName, 
                null, 
                model, 
                null
            );
            var result = await _internalClient.SendInternalRequest<CollectionItem>(request, CancellationToken.None);
            return result;
        }

        public async Task<CollectionItem> EditItemAsync(object model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Put, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                _apiClientSettings.EditItemMethodName, 
                null, 
                model, 
                null
            );
            var result = await _internalClient.SendInternalRequest<CollectionItem>(request, CancellationToken.None);
            return result;
        }

        public async Task<string> RemoveItemAsync(string userId, string id)
        {
            var request = new InternalRequest
            (
                HttpMethod.Delete, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                _apiClientSettings.RemoveItemMethodName,
                new string[] 
                {
                    userId, 
                    id 
                }, 
                null, 
                null
            );
            var result = await _internalClient.SendInternalRequest<string>(request, CancellationToken.None);
            return result;
        }
    }
}