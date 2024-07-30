using System;
using System.Diagnostics.CodeAnalysis;
using FitnessApp.ApiGateway.Enums.UserProfile;
using Newtonsoft.Json;

namespace FitnessApp.ApiGateway.Contracts.UserProfile.Output;

[ExcludeFromCodeCoverage]
public class UsersProfilesShortContract
{
    [JsonRequired]
    public string UserId { get; set; }
    [JsonRequired]
    public string FirstName { get; set; }
    [JsonRequired]
    public string LastName { get; set; }
    [JsonRequired]
    public string CroppedProfilePhoto { get; set; }
    [JsonRequired]
    public DateTime BirthDate { get; set; }
    [JsonRequired]
    public Gender Gender { get; set; }
    [JsonRequired]
    public string About { get; set; }
    [JsonRequired]
    public bool CanFollow { get; set; }
}