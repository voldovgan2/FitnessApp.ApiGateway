using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Models.Contacts.Output;

[ExcludeFromCodeCoverage]
public class FollowerStatusModel
{
    public string UserId { get; set; }
    public bool IsFollower { get; set; }
}
