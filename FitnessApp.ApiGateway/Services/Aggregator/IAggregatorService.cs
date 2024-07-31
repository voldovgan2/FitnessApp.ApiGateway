using System.Threading.Tasks;
using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Exercises.Input;
using FitnessApp.ApiGateway.Models.Exercises.Output;
using FitnessApp.ApiGateway.Models.Food.Input;
using FitnessApp.ApiGateway.Models.Food.Output;
using FitnessApp.ApiGateway.Models.Notification;
using FitnessApp.ApiGateway.Models.Settings.Input;
using FitnessApp.ApiGateway.Models.Settings.Output;
using FitnessApp.ApiGateway.Models.UserProfile.Input;
using FitnessApp.ApiGateway.Models.UserProfile.Output;
using FitnessApp.Common.Paged.Models.Output;

namespace FitnessApp.ApiGateway.Services.Aggregator;

public interface IAggregatorService
{
    #region Contacts
    Task<PagedDataModel<UserProfileModel>> GetUserContacts(GetUserContactsModel model);
    Task<string> StartFollow(SendFollowModel model);
    Task<string> AcceptFollowRequest(ProcessFollowRequestModel model);
    Task<string> RejectFollowRequest(ProcessFollowRequestModel model);
    Task<string> DeleteFollowRequest(SendFollowModel model);
    Task<string> DeleteFollower(ProcessFollowRequestModel model);
    Task<string> UnfollowUser(SendFollowModel model);

    #endregion

    #region Settings

    Task<SettingsModel> GetSettings(string userId);
    Task<SettingsModel> CreateSettings(CreateSettingsModel model);
    Task<SettingsModel> UpdateSettings(UpdateSettingsModel model);
    Task<string> DeleteSettings(string userId);

    #endregion

    #region UserProfile

    Task<UserProfileModel> GetUserProfile(GetUserProfileModel model);
    Task<UserProfileModel> CreateUserProfile(CreateUserProfileModel model);
    Task<UserProfileModel> UpdateUserProfile(UpdateUserProfileModel model);
    Task<string> DeleteUserProfile(string userId);

    #endregion

    #region Food

    Task<UserFoodsModel> GetFoods(GetUserFoodsModel model);
    Task<FoodItemModel> AddFood(AddUserFoodModel model);
    Task<FoodItemModel> EditFood(UpdateUserFoodModel model);
    Task<string> RemoveFood(string userId, string foodId);

    #endregion

    #region Exercises

    Task<UserExercisesModel> GetExercises(GetUserExercisesModel model);
    Task<ExerciseItemModel> AddExercise(AddUserExerciseModel model);
    Task<ExerciseItemModel> EditExercise(UpdateUserExerciseModel model);
    Task<string> RemoveExercise(string userId, string exerciseId);

    #endregion

    #region NotificationService

    Task<string> GetNotificationTicket(NotificationTicketModel model);

    Task<bool> ValidateNotificationTicket(ValidateNotificationTicketModel model);

    #endregion
}
