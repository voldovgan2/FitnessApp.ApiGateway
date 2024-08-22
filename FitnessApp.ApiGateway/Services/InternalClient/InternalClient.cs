using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Extensions;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Services.Authorization;
using Newtonsoft.Json;

namespace FitnessApp.ApiGateway.Services.InternalClient;

public class InternalClient(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider) : IInternalClient
{
    public async Task<TResponse> SendInternalRequest<TResponse>(string apiName, string scope, InternalRequest internalRequest)
    {
        var request = await CreateRequest(apiName, scope, internalRequest);
        var internalHttpClient = httpClientFactory.CreateClient("InternalClient");
        var response = await internalHttpClient.SendAsync(request);
        return await ProcessResponse<TResponse>(response);
    }

    private async Task<HttpRequestMessage> CreateRequest(string apiName, string scope, InternalRequest internalRequest)
    {
        var url = internalRequest.BaseUrl
            .Api(internalRequest.Api).Method(internalRequest.Method)
            .Routes(internalRequest.Routes)
            .ToQueryString(internalRequest.Query);

        var request = new HttpRequestMessage(internalRequest.HttpMethod, url);
        if (internalRequest.Payload != null)
            request.Content = new StringContent(JsonConvert.SerializeObject(internalRequest.Payload), Encoding.UTF8, "application/json");

        var token = await tokenProvider.GetAuthenticationToken(apiName, scope);
        request.Headers.Add("Authorization", $"Bearer {token}");

        return request;
    }

    private async Task<TResponse> ProcessResponse<TResponse>(HttpResponseMessage httpResponseMessage)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        httpResponseMessage.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<TResponse>(content);
    }
}