using System.Net.Http;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.Common.Paged.Models.Output;

namespace FitnessApp.ApiGateway.Services.Abstractions;

public abstract class GenericService<TModel>(ApiClientSettings apiClientSettings, IInternalClient internalClient)
    : BaseService(apiClientSettings, internalClient)
{
    protected async Task<PagedDataModel<TModel>> GetItems(string baseUrl, string api, string methodName, object payload)
    {
        var request = new InternalRequest(
            HttpMethod.Get,
            baseUrl,
            api,
            methodName,
            null,
            null,
            payload);
        var result = await InternalClient.SendInternalRequest<PagedDataModel<TModel>>(ApiClientSettings.ApiName, ApiClientSettings.Scope, request);
        return result;
    }

    protected async Task<TModel> GetItem(string baseUrl, string api, string methodName, string userId)
    {
        var request = new InternalRequest(
            HttpMethod.Get,
            baseUrl,
            api,
            methodName,
            [userId],
            null,
            null);
        var result = await InternalClient.SendInternalRequest<TModel>(ApiClientSettings.ApiName, ApiClientSettings.Scope, request);
        return result;
    }

    protected async Task<TModel> CreateItem(string baseUrl, string api, string methodName, object payload)
    {
        var request = new InternalRequest(
            HttpMethod.Post,
            baseUrl,
            api,
            methodName,
            null,
            payload,
            null);
        var result = await InternalClient.SendInternalRequest<TModel>(ApiClientSettings.ApiName, ApiClientSettings.Scope, request);
        return result;
    }

    protected async Task<TModel> UpdateItem(string baseUrl, string api, string methodName, object payload)
    {
        var request = new InternalRequest(
            HttpMethod.Put,
            baseUrl,
            api,
            methodName,
            null,
            payload,
            null);
        var result = await InternalClient.SendInternalRequest<TModel>(ApiClientSettings.ApiName, ApiClientSettings.Scope, request);
        return result;
    }

    protected async Task<string> DeleteItem(string baseUrl, string api, string methodName, string userId)
    {
        var request = new InternalRequest(
            HttpMethod.Delete,
            baseUrl,
            api,
            methodName,
            [userId],
            null,
            null);
        var result = await InternalClient.SendInternalRequest<string>(ApiClientSettings.ApiName, ApiClientSettings.Scope, request);
        return result;
    }
}