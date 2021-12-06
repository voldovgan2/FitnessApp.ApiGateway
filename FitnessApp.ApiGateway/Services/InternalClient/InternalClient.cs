using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Extensions;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.Serializer.JsonSerializer;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.InternalClient
{
    public class InternalClient : IInternalClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly string _scope;
        private readonly SemaphoreSlim _locker;
        private readonly IDistributedCache _distributedCache;
        private readonly string _cacheKey;
        private readonly double _expirationCoefitient = 0.8;
        private readonly IJsonSerializer _serializer;
        private readonly ILogger<InternalClient> _logger;
               
        public InternalClient
        (
            IHttpClientFactory httpClientFactory,
            IDistributedCache distributedCache,
            AuthenticationSettings authenticationSettings,
            string apiName,
            string scope,
            IJsonSerializer serializer,
            ILoggerFactory loggerFactory
        )
        {
            _httpClientFactory = httpClientFactory;
            _distributedCache = distributedCache;
            _authenticationSettings = authenticationSettings;
            _scope = scope;
            _cacheKey = $"Authentication_{apiName}_{nameof(InternalClient)}";
            _locker = new SemaphoreSlim(1, 1);
            _serializer = serializer;
            _logger = loggerFactory.CreateLogger<InternalClient>();
        }

        public async Task<TResponse> SendInternalRequest<TResponse>(InternalRequest internalRequest, CancellationToken cancelationToken)
        {
            TResponse result = default;
            var token = await GetAuthenticationToken(cancelationToken);
            var url = internalRequest.BaseUrl.Api(internalRequest.Api).Method(internalRequest.Method);
            if(internalRequest.Routes != null)
            {
                url = url.Routes(internalRequest.Routes);
            }
            if (internalRequest.Query != null)
            {
                url = url.ToQueryString(internalRequest.Query);
            }
            var request = new HttpRequestMessage(internalRequest.HttpMethod, url);
            if(internalRequest.Payload != null)
            {
                request.Content = new StringContent(_serializer.SerializeToString(internalRequest.Payload), Encoding.UTF8, "application/json");
            }
            request.Headers.Add("Authorization", $"Bearer {token}");
            var internalHttpClient = _httpClientFactory.CreateClient("InternalClient");
            var response = await internalHttpClient.SendAsync(request, cancelationToken);
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

        private async Task<string> GetAuthenticationToken(CancellationToken cancelationToken)
        {
            var result = "";
            await _locker.WaitAsync(cancelationToken);
            try
            {
                result = await _distributedCache.GetStringAsync(_cacheKey, cancelationToken);
                if (string.IsNullOrEmpty(result))
                {
                    var tokenHttpClient = _httpClientFactory.CreateClient("TokenClient");
                    var disco = await tokenHttpClient.GetDiscoveryDocumentAsync();
                    if (disco.IsError)
                    {
                        throw disco.Exception;
                    }
                    else
                    {
                        var tokenRequest = new ClientCredentialsTokenRequest
                        {
                            Address = disco.TokenEndpoint,
                            ClientId = _authenticationSettings.ClientId,
                            ClientSecret = _authenticationSettings.ClientSecret,
                            Scope = _scope
                        };
                        var tokenResponse = await tokenHttpClient.RequestClientCredentialsTokenAsync(tokenRequest, cancelationToken);
                        var cacheOptions = new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(tokenResponse.ExpiresIn * _expirationCoefitient)
                        };
                        await _distributedCache.SetStringAsync(_cacheKey, tokenResponse.AccessToken, cacheOptions);
                        result = tokenResponse.AccessToken;
                    }
                }
            }
            finally
            {
                _locker.Release();
            }
            return result;
        }
    }
}