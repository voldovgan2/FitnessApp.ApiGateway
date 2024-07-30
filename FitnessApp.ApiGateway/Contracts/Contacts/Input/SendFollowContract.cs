using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Contracts.Contacts.Input;

[ExcludeFromCodeCoverage]
public class SendFollowContract
{
    public string UserToFollowId { get; set; }
}
