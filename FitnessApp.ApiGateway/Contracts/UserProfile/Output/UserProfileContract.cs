using System;
using FitnessApp.ApiGateway.Enums.UserProfile;
using Newtonsoft.Json;

namespace FitnessApp.ApiGateway.Contracts.UserProfile.Output;

public class UserProfileContract
{
    [JsonRequired]
    public int FollowersCount { get; set; }
    [JsonRequired]
    public int FollowingsCount { get; set; }
    [JsonRequired]
    public string Email { get; set; }
    [JsonRequired]
    public string FirstName { get; set; }
    [JsonRequired]
    public string LastName { get; set; }
    [JsonRequired]
    public string CroppedProfilePhoto { get; set; }
    [JsonRequired]
    public string OriginalProfilePhoto { get; set; }
    [JsonRequired]
    public DateTime BirthDate { get; set; }
    [JsonRequired]
    public double Height { get; set; }
    [JsonRequired]
    public double Weight { get; set; }
    [JsonRequired]
    public Gender Gender { get; set; }
    [JsonRequired]
    public string About { get; set; }
    [JsonRequired]
    public string Language { get; set; }
    [JsonRequired]
    public string BackgroundPhoto { get; set; }
    [JsonRequired]
    public bool CanFollow { get; set; }
}