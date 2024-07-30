using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace FitnessApp.ApiGateway.Models.Contacts.Output;

[ExcludeFromCodeCoverage]
public class ContactModel
{
    [JsonRequired]
    public string UserId { get; set; }
}
