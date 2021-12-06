using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.Paged.Models.Output;
using FitnessApp.Serializer.JsonSerializer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks; 

namespace FitnessApp.ApiGateway.Services.Abstractions.Base
{
    public class GenericService<Model> : IGenericService<Model>
    {
        private readonly IInternalClient _internalClient;
        private readonly ApiClientSettings _apiClientSettings;

        public GenericService
        (
            string name,
            IHttpClientFactory httpClientFactory,
            IDistributedCache distributedCache,
            IOptions<AuthenticationSettings> authenticationSettings,
            IOptionsMonitor<ApiClientSettings> optionsMonitor,
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

        public async Task<PagedDataModel<Model>> GetItemsAsync(object model)
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
            var result = await _internalClient.SendInternalRequest<PagedDataModel<Model>>(request, CancellationToken.None);
            return result;
        }

        public async Task<Model> GetItemAsync(string userId)
        {
            var request = new InternalRequest
            (
                HttpMethod.Get, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                _apiClientSettings.GetItemMethodName,
                new string[] { userId }, 
                null, 
                null
            );
            var result = await _internalClient.SendInternalRequest<Model>(request, CancellationToken.None);
            return result;
        }

        public async Task<Model> CreateItemAsync(object model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Post, 
                _apiClientSettings.Url,
                _apiClientSettings.ApiName, 
                _apiClientSettings.CreateItemMethodName, 
                null, 
                model, 
                null
            );
            var result = await _internalClient.SendInternalRequest<Model>(request, CancellationToken.None);
            return result;
        }

        public async Task<Model> UpdateItemAsync(object model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Put, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                _apiClientSettings.UpdateItemMethodName,
                null, 
                model, 
                null
            );
            var result = await _internalClient.SendInternalRequest<Model>(request, CancellationToken.None);
            return result;
        }

        public async Task<string> DeleteItemAsync(string userId)
        {
            var request = new InternalRequest
            (
                HttpMethod.Delete, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                _apiClientSettings.DeleteItemMethodName, 
                new string[] { userId }, 
                null, 
                null
            );
            var result = await _internalClient.SendInternalRequest<string>(request, CancellationToken.None);
            return result;
        }
    }
}