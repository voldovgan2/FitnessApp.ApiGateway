using System;
using FitnessApp.ApiGateway.Enums.UserProfile;
using FitnessApp.ApiGateway.Models.File.Output;

namespace FitnessApp.ApiGateway.Models.UserProfile.Output
{
    public class UserProfileModel : IFileModel
    {
        required public int FollowersCount { get; set; }
        required public int FollowingsCount { get; set; }
        required public string Id { get; set; }
        required public string UserId { get; set; }
        required public string Email { get; set; }
        required public string FirstName { get; set; }
        required public string LastName { get; set; }
        required public string CroppedProfilePhoto { get; set; }
        required public string OriginalProfilePhoto { get; set; }
        required public DateTime BirthDate { get; set; }
        required public double Height { get; set; }
        required public double Weight { get; set; }
        required public Gender Gender { get; set; }
        required public string About { get; set; }
        required public string Language { get; set; }
        required public string BackgroundPhoto { get; set; }
        required public bool CanFollow { get; set; }
    }
}
