using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Models.UserProfile.Input;

[ExcludeFromCodeCoverage]
public class GetSelectedUsersProfilesModel
{
    public IEnumerable<string> UsersIds { get; set; }
}
