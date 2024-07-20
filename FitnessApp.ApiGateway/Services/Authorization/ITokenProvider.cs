using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.Authorization;

public interface ITokenProvider
{
    Task<string> GetAuthenticationToken(string apiName, string scope, bool useCache = false);
}
