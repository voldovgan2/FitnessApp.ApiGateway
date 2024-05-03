using System.Net.Http;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Services.InternalClient;

namespace FitnessApp.ApiGateway.Services.Abstractions
{
    public abstract class CollectionService<TModel, TCollectionItem>(ApiClientSettings apiClientSettings, IInternalClient internalClient)
        : BaseService(apiClientSettings, internalClient)
    {
        protected async Task<TModel> GetItem(string baseUrl, string api, string methodName, object payload)
        {
            var request = new InternalRequest(
                HttpMethod.Get,
                baseUrl,
                api,
                methodName,
                null,
                null,
                payload);
            var result = await InternalClient.SendInternalRequest<TModel>(CreateAuthenticationTokenRequest(), request);
            return result;
        }

        protected async Task<TCollectionItem> AddItem(string baseUrl, string api, string methodName, object payload)
        {
            var request = new InternalRequest(
                HttpMethod.Post,
                baseUrl,
                api,
                methodName,
                null,
                payload,
                null);
            var result = await InternalClient.SendInternalRequest<TCollectionItem>(CreateAuthenticationTokenRequest(), request);
            return result;
        }

        protected async Task<TCollectionItem> EditItem(string baseUrl, string api, string methodName, object payload)
        {
            var request = new InternalRequest(
                HttpMethod.Put,
                baseUrl,
                api,
                methodName,
                null,
                payload,
                null);
            var result = await InternalClient.SendInternalRequest<TCollectionItem>(CreateAuthenticationTokenRequest(), request);
            return result;
        }

        protected async Task<string> RemoveItem(string baseUrl, string api, string methodName, string userId, string id)
        {
            var request = new InternalRequest(
                HttpMethod.Delete,
                baseUrl,
                api,
                methodName,
                [
                    userId,
                    id
                ],
                null,
                null);
            var result = await InternalClient.SendInternalRequest<string>(CreateAuthenticationTokenRequest(), request);
            return result;
        }
    }
}