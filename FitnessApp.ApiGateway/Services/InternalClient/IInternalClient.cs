using FitnessApp.ApiGateway.Models.Internal;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.InternalClient
{
    public interface IInternalClient
    {
        Task<TResponse> SendInternalRequest<TResponse>(InternalRequest internalRequest, CancellationToken cancelationToken);
    }
}
