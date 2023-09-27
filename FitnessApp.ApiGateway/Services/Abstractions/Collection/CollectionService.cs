using System.Net.Http;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Services.InternalClient;

namespace FitnessApp.ApiGateway.Services.Abstractions.Collection
{
    public class CollectionService<TModel, TCollectionItem> : ICollectionService<TModel, TCollectionItem>
    {
        private readonly IInternalClient _internalClient;
        private readonly AuthenticationTokenRequest _authenticationTokenRequest;

        public CollectionService(ApiClientSettings apiClientSettings, IInternalClient internalClient)
        {
            _internalClient = internalClient;
            _authenticationTokenRequest = new AuthenticationTokenRequest
            {
                ApiName = apiClientSettings.ApiName,
                Scope = apiClientSettings.Scope
            };
        }

        public async Task<TModel> GetItem(string baseUrl, string api, string methodName, object payload)
        {
            var request = new InternalRequest(
                HttpMethod.Get,
                baseUrl,
                api,
                methodName,
                null,
                null,
                payload);
            var result = await _internalClient.SendInternalRequest<TModel>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<TCollectionItem> AddItem(string baseUrl, string api, string methodName, object payload)
        {
            var request = new InternalRequest(
                HttpMethod.Post,
                baseUrl,
                api,
                methodName,
                null,
                payload,
                null);
            var result = await _internalClient.SendInternalRequest<TCollectionItem>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<TCollectionItem> EditItem(string baseUrl, string api, string methodName, object payload)
        {
            var request = new InternalRequest(
                HttpMethod.Put,
                baseUrl,
                api,
                methodName,
                null,
                payload,
                null);
            var result = await _internalClient.SendInternalRequest<TCollectionItem>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<string> RemoveItem(string baseUrl, string api, string methodName, string userId, string id)
        {
            var request = new InternalRequest(
                HttpMethod.Delete,
                baseUrl,
                api,
                methodName,
                new string[]
                {
                    userId,
                    id
                },
                null,
                null);
            var result = await _internalClient.SendInternalRequest<string>(_authenticationTokenRequest, request);
            return result;
        }
    }
}