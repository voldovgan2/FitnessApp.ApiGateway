using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Models.Contacts.Input;

[ExcludeFromCodeCoverage]
public class GetFollowerStatusModel
{
    public string UserId { get; set; }
    public string ContactsUserId { get; set; }
}
