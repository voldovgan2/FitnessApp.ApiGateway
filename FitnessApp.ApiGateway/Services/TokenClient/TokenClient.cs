using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Exceptions;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.Common.Vault;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace FitnessApp.ApiGateway.Services.TokenClient
{
    public class TokenClient : ITokenClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiAuthenticationSettings _authenticationSettings;
        private readonly SemaphoreSlim _locker;
        private readonly IDistributedCache _distributedCache;
        private const double EXPIRATION_COEFITIENT = 0.8;

        public TokenClient(
            IHttpClientFactory httpClientFactory,
            IOptions<ApiAuthenticationSettings> authenticationSettings,
            IVaultService vaultService,
            IDistributedCache distributedCache)
        {
            _httpClientFactory = httpClientFactory;
            _authenticationSettings = authenticationSettings.Value;
            _authenticationSettings.ClientSecret = vaultService.GetSecret("ApiAuthenticationSettings:domain_client_secret")
                .GetAwaiter().GetResult();
            _distributedCache = distributedCache;
            _locker = new SemaphoreSlim(1, 1);
        }

        public async Task<string> GetAuthenticationToken(AuthenticationTokenRequest authenticationTokenRequest, bool useCache = false)
        {
            return useCache ?
                await GetTokenFromCache(authenticationTokenRequest.ApiName, authenticationTokenRequest.Scope)
                : (await RequestAuthenticationToken(authenticationTokenRequest.Scope)).AccessToken;
        }

        private async Task<string> GetTokenFromCache(string apiName, string scope)
        {
            var result = "";
            await _locker.WaitAsync();
            try
            {
                var cacheKey = $"Authentication_{apiName}_{nameof(TokenClient)}";
                result = await _distributedCache.GetStringAsync(cacheKey);
                if (string.IsNullOrEmpty(result))
                {
                    var tokenResponse = await RequestAuthenticationToken(scope);
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(tokenResponse.ExpiresIn * EXPIRATION_COEFITIENT)
                    };
                    await _distributedCache.SetStringAsync(cacheKey, tokenResponse.AccessToken, cacheOptions);
                    result = tokenResponse.AccessToken;
                }
            }
            finally
            {
                _locker.Release();
            }

            return result;
        }

        private async Task<TokenResponse> RequestAuthenticationToken(string scope)
        {
            var tokenHttpClient = _httpClientFactory.CreateClient("TokenClient");
            var disco = await tokenHttpClient.GetDiscoveryDocumentAsync(
                new DiscoveryDocumentRequest
                {
                    Address = _authenticationSettings.Address,
                    Policy =
                    {
                        RequireHttps = false
                    }
                });
            if (disco.IsError)
            {
                throw disco.Exception;
            }
            else
            {
                var tokenResponse = await tokenHttpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = _authenticationSettings.ClientId,
                    ClientSecret = _authenticationSettings.ClientSecret,
                    Scope = scope
                });
                if (tokenResponse.IsError)
                    throw new InternalUnAuthorizedException(tokenResponse.Error);

                return tokenResponse;
            }
        }
    }
}