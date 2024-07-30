using System.Diagnostics.CodeAnalysis;
using FitnessApp.ApiGateway.Enums.Contacts;
using FitnessApp.Common.Paged.Contracts.Input;

namespace FitnessApp.ApiGateway.Contracts.Contacts.Input;

[ExcludeFromCodeCoverage]
public class GetUserContactsContract : GetPagedDataContract
{
    public string ContactsUserId { get; set; }
    public ContactsType ContactsType { get; set; }
}