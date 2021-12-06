namespace FitnessApp.ApiGateway.Models.Contacts.Input
{
    public class SendFollowModel
    {
        public string UserId { get; set; }
        public string UserToFollowId { get; set; }
    }
}