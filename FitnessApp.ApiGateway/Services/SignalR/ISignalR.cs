using System.Threading.Tasks;
using FitnessApp.ApiGateway.Models.SignalR;

namespace FitnessApp.ApiGateway.Services.SignalR
{
    public interface ISignalR
    {
        Task<string> GetToken();
        Task SendMessage(FollowRequestConfirmedModel model);
    }
}