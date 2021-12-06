using FitnessApp.ApiGateway.Enums.UserProfile;
using System;

namespace FitnessApp.ApiGateway.Models.UserProfile.Input
{
    public class CreateUserProfileModel
    {
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
    }
}
