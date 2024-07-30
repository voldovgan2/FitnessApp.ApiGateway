using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Models.UserProfile.Input;

[ExcludeFromCodeCoverage]
public class GetUserProfileModel
{
    public string UserId { get; set; }
    public string ContactsUserId { get; set; }
}