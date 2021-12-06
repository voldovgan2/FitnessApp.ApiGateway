using FitnessApp.ApiGateway.Enums.UserProfile;
using System;

namespace FitnessApp.ApiGateway.Contracts.UserProfile.Output
{
    public class UsersProfilesShortContract
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CroppedProfilePhoto { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string About { get; set; }
        public bool CanFollow { get; set; }
    }
}