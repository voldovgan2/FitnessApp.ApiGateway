using System.Threading.Tasks;
using FitnessApp.ApiGateway.Models.Notification;
using FitnessApp.Common.ServiceBus.Nats.Events;

namespace FitnessApp.ApiGateway.Services.NotificationService;

public interface INotificationService
{
    Task<string> GetNotificationTicket(NotificationTicketModel model);
    Task<bool> ValidateNotificationTicket(ValidateNotificationTicketModel model);
    Task SendMessage(FollowRequestConfirmed model);
}