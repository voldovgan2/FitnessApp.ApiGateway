using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Contracts.Contacts.Input;

[ExcludeFromCodeCoverage]
public class ProcessFollowRequestContract
{
    public string UserId { get; set; }
    public string FollowerUserId { get; set; }
}
