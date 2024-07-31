using System.Net.Http;
using System.Threading.Tasks;
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
        var discoveryDocumentResponse = await tokenHttpClient.GetDiscoveryDocumentAsync(
            new DiscoveryDocumentRequest
            {
                Address = address,
                Policy =
                {
                    RequireHttps = false
                }
            });
        EnsureProtocolResponseStatusCode(discoveryDocumentResponse);
        var tokenResponse = await tokenHttpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryDocumentResponse.TokenEndpoint,
            ClientId = clientId,
            ClientSecret = clientSecret,
            Scope = scope
        });
        EnsureProtocolResponseStatusCode(tokenResponse);
        return (tokenResponse.AccessToken, tokenResponse.ExpiresIn);
    }

    private void EnsureProtocolResponseStatusCode(ProtocolResponse protocolResponse)
    {
        if (protocolResponse.IsError)
        {
            throw protocolResponse.Exception;
        }
    }
}
