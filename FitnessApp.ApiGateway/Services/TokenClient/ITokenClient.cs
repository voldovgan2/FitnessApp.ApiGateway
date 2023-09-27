using System.Threading.Tasks;
using FitnessApp.ApiGateway.Models.Internal;

namespace FitnessApp.ApiGateway.Services.TokenClient
{
    public interface ITokenClient
    {
        Task<string> GetAuthenticationToken(AuthenticationTokenRequest authenticationTokenRequest, bool useCache = false);
    }
}
