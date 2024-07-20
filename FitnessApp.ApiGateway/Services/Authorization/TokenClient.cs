using System.Net.Http;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Exceptions;
using IdentityModel.Client;

namespace FitnessApp.ApiGateway.Services.Authorization;

public class TokenClient(IHttpClientFactory httpClientFactory) : ITokenClient
{
    public async Task<(string AccessToken, int ExpiresIn)> GetAuthenticationToken(
        string address,
        string clientId,
        string clientSecret,
        string scope)
    {
        var tokenHttpClient = httpClientFactory.CreateClient("TokenClient");
        var disco = await tokenHttpClient.GetDiscoveryDocumentAsync(
            new DiscoveryDocumentRequest
            {
                Address = address,
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
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope
            });
            if (tokenResponse.IsError)
                throw new InternalUnAuthorizedException(tokenResponse.Error);

            return (tokenResponse.AccessToken, tokenResponse.ExpiresIn);
        }
    }
}
