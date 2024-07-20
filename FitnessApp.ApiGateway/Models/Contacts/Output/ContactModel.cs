using Newtonsoft.Json;

namespace FitnessApp.ApiGateway.Models.Contacts.Output;

public class ContactModel
{
    [JsonRequired]
    public string UserId { get; set; }
}
