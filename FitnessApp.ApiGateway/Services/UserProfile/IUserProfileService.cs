using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Models.UserProfile.Input;
using FitnessApp.ApiGateway.Models.UserProfile.Output;

namespace FitnessApp.ApiGateway.Services.UserProfile
{
    public interface IUserProfileService
    {
        Task<UserProfileModel> GetUserProfile(string userId);
        Task<UserProfileModel> CreateUserProfile(CreateUserProfileModel model);
        Task<UserProfileModel> UpdateUserProfile(UpdateUserProfileModel model);
        Task<string> DeleteUserProfile(string userId);
        Task<IEnumerable<UserProfileModel>> GetUsersProfiles(GetSelectedUsersProfilesModel model);
    }
}