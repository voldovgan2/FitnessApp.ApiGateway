using System.Collections.Generic;

namespace FitnessApp.ApiGateway.Models.Contacts.Input;

public class GetFollowersStatusModel
{
    public IEnumerable<string> UserIds { get; set; }
    public string ContactsUserId { get; set; }
}
