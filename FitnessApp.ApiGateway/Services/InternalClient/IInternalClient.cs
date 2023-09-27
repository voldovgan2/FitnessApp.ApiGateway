using System.Threading.Tasks;
using FitnessApp.ApiGateway.Models.Internal;

namespace FitnessApp.ApiGateway.Services.InternalClient
{
    public interface IInternalClient
    {
        Task<TResponse> SendInternalRequest<TResponse>(
            AuthenticationTokenRequest authenticationTokenRequest,
            InternalRequest internalRequest);
    }
}
