namespace FitnessApp.ApiGateway.Models.Notification;

public class ValidateNotificationTicketModel
{
    public string Ticket { get; set; }
    public string Ip { get; set; }
    public string UserId { get; set; }
}
