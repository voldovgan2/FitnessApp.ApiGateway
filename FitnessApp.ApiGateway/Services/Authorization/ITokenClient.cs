using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.Authorization;

public interface ITokenClient
{
    Task<(string AccessToken, int ExpiresIn)> GetAuthenticationToken(
        string address,
        string clientId,
        string clientSecret,
        string scope);
}
