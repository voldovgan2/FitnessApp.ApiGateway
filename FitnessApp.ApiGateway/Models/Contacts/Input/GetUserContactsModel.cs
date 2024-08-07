﻿using System.Diagnostics.CodeAnalysis;
using FitnessApp.ApiGateway.Enums.Contacts;
using FitnessApp.Common.Paged.Models.Input;

namespace FitnessApp.ApiGateway.Models.Contacts.Input;

[ExcludeFromCodeCoverage]
public class GetUserContactsModel : GetPagedDataModel
{
    public string UserId { get; set; }
    public string ContactsUserId { get; set; }
    public ContactsType ContactsType { get; set; }
}