using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Models.Contacts.Input;

[ExcludeFromCodeCoverage]
public class ProcessFollowRequestModel
{
    public string UserId { get; set; }
    public string FollowerUserId { get; set; }
}