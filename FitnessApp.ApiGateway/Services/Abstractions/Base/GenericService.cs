using System.Net.Http;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.Common.Paged.Models.Output;

namespace FitnessApp.ApiGateway.Services.Abstractions.Base
{
    public class GenericService<TModel> : IGenericService<TModel>
    {
        private readonly IInternalClient _internalClient;
        private readonly AuthenticationTokenRequest _authenticationTokenRequest;

        public GenericService(ApiClientSettings apiClientSettings, IInternalClient internalClient)
        {
            _internalClient = internalClient;
            _authenticationTokenRequest = new AuthenticationTokenRequest
            {
                ApiName = apiClientSettings.ApiName,
                Scope = apiClientSettings.Scope
            };
        }

        public async Task<PagedDataModel<TModel>> GetItems(string baseUrl, string api, string methodName, object payload)
        {
            var request = new InternalRequest(
                HttpMethod.Get,
                baseUrl,
                api,
                methodName,
                null,
                null,
                payload);
            var result = await _internalClient.SendInternalRequest<PagedDataModel<TModel>>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<TModel> GetItem(string baseUrl, string api, string methodName, string userId)
        {
            var request = new InternalRequest(
                HttpMethod.Get,
                baseUrl,
                api,
                methodName,
                new string[] { userId },
                null,
                null);
            var result = await _internalClient.SendInternalRequest<TModel>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<TModel> CreateItem(string baseUrl, string api, string methodName, object payload)
        {
            var request = new InternalRequest(
                HttpMethod.Post,
                baseUrl,
                api,
                methodName,
                null,
                payload,
                null);
            var result = await _internalClient.SendInternalRequest<TModel>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<TModel> UpdateItem(string baseUrl, string api, string methodName, object payload)
        {
            var request = new InternalRequest(
                HttpMethod.Put,
                baseUrl,
                api,
                methodName,
                null,
                payload,
                null);
            var result = await _internalClient.SendInternalRequest<TModel>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<string> DeleteItem(string baseUrl, string api, string methodName, string userId)
        {
            var request = new InternalRequest(
                HttpMethod.Delete,
                baseUrl,
                api,
                methodName,
                new string[] { userId },
                null,
                null);
            var result = await _internalClient.SendInternalRequest<string>(_authenticationTokenRequest, request);
            return result;
        }
    }
}