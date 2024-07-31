using System;
using System.Threading;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.Common.Vault;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace FitnessApp.ApiGateway.Services.Authorization;

public class TokenProvider : ITokenProvider
{
    private readonly ApiAuthenticationSettings _authenticationSettings;
    private readonly ITokenClient _tokenClient;
    private readonly SemaphoreSlim _locker;
    private readonly IDistributedCache _distributedCache;
    private const double EXPIRATION_COEFITIENT = 0.8;

    public TokenProvider(
        ITokenClient tokenClient,
        IVaultService vaultService,
        IOptions<ApiAuthenticationSettings> authenticationSettings,
        IDistributedCache distributedCache)
    {
        _tokenClient = tokenClient;
        _authenticationSettings = authenticationSettings.Value;
        _authenticationSettings.ClientSecret = vaultService
            .GetSecret("ApiAuthenticationSettings:ClientSecret")
            .GetAwaiter()
            .GetResult();

        _distributedCache = distributedCache;
        _locker = new SemaphoreSlim(1, 1);
    }

    public async Task<string> GetAuthenticationToken(string apiName, string scope, bool useCache = false)
    {
        return useCache ?
            await GetTokenFromCache(apiName, scope)
            : (await _tokenClient.GetAuthenticationToken(
                _authenticationSettings.Address,
                _authenticationSettings.ClientId,
                _authenticationSettings.ClientSecret,
                scope)).AccessToken;
    }

    private async Task<string> GetTokenFromCache(string apiName, string scope)
    {
        var result = "";
        await _locker.WaitAsync();
        try
        {
            var cacheKey = $"Authentication_{apiName}_{nameof(TokenProvider)}";
            result = await _distributedCache.GetStringAsync(cacheKey);
            if (string.IsNullOrEmpty(result))
            {
                var (AccessToken, ExpiresIn) = await _tokenClient.GetAuthenticationToken(
                    _authenticationSettings.Address,
                    _authenticationSettings.ClientId,
                    _authenticationSettings.ClientSecret,
                    scope);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ExpiresIn * EXPIRATION_COEFITIENT)
                };
                await _distributedCache.SetStringAsync(cacheKey, AccessToken, cacheOptions);
                result = AccessToken;
            }
        }
        finally
        {
            _locker.Release();
        }

        return result;
    }
}