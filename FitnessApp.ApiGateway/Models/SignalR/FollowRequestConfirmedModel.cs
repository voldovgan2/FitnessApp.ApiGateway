namespace FitnessApp.ApiGateway.Models.SignalR
{
    public class FollowRequestConfirmedModel
    {
        // UserId
        public string Sender { get; set; }

        // UserToFollowId
        public string Receiver { get; set; }
    }
}
