using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Models.Contacts.Input;

[ExcludeFromCodeCoverage]
public class SendFollowModel
{
    public string UserId { get; set; }
    public string UserToFollowId { get; set; }
}