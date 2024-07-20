using System.Collections.Generic;

namespace FitnessApp.ApiGateway.Models.UserProfile.Input;

public class GetSelectedUsersProfilesModel
{
    public IEnumerable<string> UsersIds { get; set; }
}
