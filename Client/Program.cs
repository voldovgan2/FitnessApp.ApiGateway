using IdentityModel.Client;
using Newtonsoft.Json;
using SvTest;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

var token = await RequestAuthenticationToken("api.settings");
if (token.IsError)
    return;

var authMethod = new TokenAuthMethodInfo("dev-only-token");
#pragma warning disable S1075 // URIs should not be hardcoded
var config = new VaultClientSettings("http://localhost:8200", authMethod);
#pragma warning restore S1075 // URIs should not be hardcoded
var vaultClient = new VaultClient(config);
var data = new Dictionary<string, object>();
data.Add("Minio:SecretKey", "minio_password");
if (data.Count == 1)
    await vaultClient.V1.Secrets.KeyValue.V1.WriteSecretAsync("fitness-app", data);
var savaTest = await vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync("fitness-app");
Console.WriteLine(savaTest);
Console.ReadLine();

async Task<TokenResponse> RequestAuthenticationToken(string scope)
{
    var tokenHttpClient = new HttpClient();
    var disco = await tokenHttpClient.GetDiscoveryDocumentAsync(
        new DiscoveryDocumentRequest
        {
            Address = "http://localhost:5000",
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
            ClientId = "domain_client",
            ClientSecret = "domain_client_secret",
            Scope = scope
        });

        return tokenResponse;
    }
}

namespace SvTest
{
    public class TestCollectionModel
    {
        public string UserId { get; set; } = "";
        public Dictionary<string, List<TestCollectionItemModel>> Collection { get; set; } = new Dictionary<string, List<TestCollectionItemModel>>();
    }

    public class TestCollectionItemModel
    {
        public string Id { get; set; } = "";
    }
}
