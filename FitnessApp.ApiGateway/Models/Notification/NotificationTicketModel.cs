using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Models.Notification;

[ExcludeFromCodeCoverage]
public class NotificationTicketModel
{
    public string Ip { get; set; }
    public string UserId { get; set; }
}
