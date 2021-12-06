using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Exercises.Input;
using FitnessApp.ApiGateway.Models.Exercises.Output;
using FitnessApp.ApiGateway.Models.Food.Input;
using FitnessApp.ApiGateway.Models.Food.Output;
using FitnessApp.ApiGateway.Models.Settings.Input;
using FitnessApp.ApiGateway.Models.Settings.Output;
using FitnessApp.ApiGateway.Models.UserProfile.Input;
using FitnessApp.ApiGateway.Models.UserProfile.Output;
using FitnessApp.Paged.Models.Output;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.Aggregator
{
    public interface IAggregatorService
    {
        #region Contacts

        Task<bool> CanViewUserContactsAsync(GetUserContactsModel model);
        Task<PagedDataModel<UserProfileModel>> GetUserContactsAsync(GetUserContactsModel model);
        Task<string> StartFollowAsync(SendFollowModel model);
        Task<string> AcceptFollowRequestAsync(ProcessFollowRequestModel model);
        Task<string> RejectFollowRequestAsync(ProcessFollowRequestModel model);
        Task<string> DeleteFollowRequestAsync(SendFollowModel model);
        Task<string> DeleteFollowerAsync(ProcessFollowRequestModel model);
        Task<string> UnfollowUserAsync(SendFollowModel model);

        #endregion

        #region Settings

        Task<SettingsModel> GetSettingsAsync(string userId);
        Task<SettingsModel> CreateSettingsAsync(CreateSettingsModel model);
        Task<SettingsModel> UpdateSettingsAsync(UpdateSettingsModel model);
        Task<string> DeleteSettingsAsync(string userId);

        #endregion

        #region UserProfile

        Task<UserProfileModel> GetUserProfileAsync(GetUserProfileModel model);
        Task<UserProfileModel> CreateUserProfileAsync(CreateUserProfileModel model);
        Task<UserProfileModel> UpdateUserProfileAsync(UpdateUserProfileModel model);
        Task<string> DeleteUserProfileAsync(string userId);

        #endregion

        #region Food

        Task<UserFoodsModel> GetFoodsAsync(GetUserFoodsModel userId);
        Task<FoodItemModel> AddFoodAsync(AddUserFoodModel model);
        Task<FoodItemModel> EditFoodAsync(UpdateUserFoodModel model);
        Task<string> RemoveFoodAsync(string userId, string foodId);

        #endregion

        #region Exercises

        Task<UserExercisesModel> GetExercisesAsync(GetUserExercisesModel model);
        Task<ExerciseItemModel> AddExerciseAsync(AddUserExerciseModel model);
        Task<ExerciseItemModel> EditExerciseAsync(UpdateUserExerciseModel model);
        Task<string> RemoveExerciseAsync(string userId, string exerciseId);

        #endregion
    }
}
