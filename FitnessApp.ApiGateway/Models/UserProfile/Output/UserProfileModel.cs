using System;
using FitnessApp.ApiGateway.Enums.UserProfile;
using FitnessApp.ApiGateway.Models.Blob.Output;

namespace FitnessApp.ApiGateway.Models.UserProfile.Output
{
    public class UserProfileModel : IBlobModel
    {
        public int FollowersCount { get; set; }
        public int FollowingsCount { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CroppedProfilePhoto { get; set; }
        public string OriginalProfilePhoto { get; set; }
        public DateTime BirthDate { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public Gender Gender { get; set; }
        public string About { get; set; }
        public string Language { get; set; }
        public string BackgroundPhoto { get; set; }
        public bool CanFollow { get; set; }
    }
}
