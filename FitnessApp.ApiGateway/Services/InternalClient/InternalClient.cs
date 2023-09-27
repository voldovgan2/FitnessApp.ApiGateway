using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Extensions;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Services.TokenClient;
using FitnessApp.Common.Serializer.JsonSerializer;
using Microsoft.Extensions.Logging;

namespace FitnessApp.ApiGateway.Services.InternalClient
{
    public class InternalClient : IInternalClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenClient _tokenClient;
        private readonly IJsonSerializer _serializer;
        private readonly ILogger<InternalClient> _logger;

        public InternalClient(
            IHttpClientFactory httpClientFactory,
            ITokenClient tokenClient,
            IJsonSerializer serializer,
            ILoggerFactory loggerFactory)
        {
            _httpClientFactory = httpClientFactory;
            _tokenClient = tokenClient;
            _serializer = serializer;
            _logger = loggerFactory.CreateLogger<InternalClient>();
        }

        public async Task<TResponse> SendInternalRequest<TResponse>(
            AuthenticationTokenRequest authenticationTokenRequest,
            InternalRequest internalRequest)
        {
            TResponse result = default;
            var token = await _tokenClient.GetAuthenticationToken(authenticationTokenRequest);
            var url = internalRequest.BaseUrl.Api(internalRequest.Api).Method(internalRequest.Method);
            if (internalRequest.Routes != null)
                url = url.Routes(internalRequest.Routes);

            if (internalRequest.Query != null)
                url = url.ToQueryString(internalRequest.Query);

            var request = new HttpRequestMessage(internalRequest.HttpMethod, url);
            if (internalRequest.Payload != null)
                request.Content = new StringContent(_serializer.SerializeToString(internalRequest.Payload), Encoding.UTF8, "application/json");

            request.Headers.Add("Authorization", $"Bearer {token}");
            var internalHttpClient = _httpClientFactory.CreateClient("InternalClient");
            var response = await internalHttpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                result = _serializer.DeserializeFromString<TResponse>(content);
            }
            else
            {
                var routeDetails = internalRequest.Routes == null ?
                    ""
                    : $" Route: {internalRequest.Routes}";
                _logger.LogError($"Error sending request. Api: {internalRequest.Api} Method: {internalRequest.Method}{routeDetails} Response content: {content}");
            }

            return result;
        }
    }
}