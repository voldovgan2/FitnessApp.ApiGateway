using System.Linq;
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
using FitnessApp.ApiGateway.Services.Contacts;
using FitnessApp.ApiGateway.Services.Exercises;
using FitnessApp.ApiGateway.Services.Food;
using FitnessApp.ApiGateway.Services.NotificationService;
using FitnessApp.ApiGateway.Services.Settings;
using FitnessApp.ApiGateway.Services.UserProfile;
using FitnessApp.Common.Paged.Extensions;
using FitnessApp.Common.Paged.Models.Output;
using FitnessApp.Common.ServiceBus.Nats.Events;

namespace FitnessApp.ApiGateway.Services.Aggregator;

public class AggregatorService(
    IContactsService contactsService,
    ISettingsService settingsService,
    IUserProfileService userProfileService,
    IFoodService foodService,
    IExercisesService exercisesService,
    INotificationService notificationService) : IAggregatorService
{
    #region Contacts

    public async Task<PagedDataModel<UserProfileModel>> GetUserContacts(GetUserContactsModel model)
    {
        PagedDataModel<UserProfileModel> result = Enumerable.Empty<UserProfileModel>().ToPaged(model);

        var canViewUserContacts = await contactsService.CanViewUserContacts(model);
        if (!canViewUserContacts)
            return result;

        var contacts = await contactsService.GetUserContacts(model);
        if (contacts == null)
            return result;

        var profiles = await userProfileService.GetUsersProfiles(new GetSelectedUsersProfilesModel
        {
            UsersIds = contacts.Select(i => i.UserId)
        });
        var followerStatusModels = await contactsService.IsFollowers(new GetFollowersStatusModel
        {
            ContactsUserId = model.UserId,
            UserIds = contacts.Select(i => i.UserId)
        });
        foreach (var profile in profiles)
        {
            profile.CanFollow = !followerStatusModels.Single(followerStatusModel => followerStatusModel.UserId == profile.UserId).IsFollower;
        }

        result = profiles.ToPaged(model);
        return result;
    }

    public Task<string> StartFollow(SendFollowModel model)
    {
        return contactsService.StartFollow(model);
    }

    public async Task<string> AcceptFollowRequest(ProcessFollowRequestModel model)
    {
        var result = await contactsService.AcceptFollowRequest(model);
        await notificationService.SendMessage(new FollowRequestConfirmed
        {
            UserId = model.UserId,
            FollowerUserId = model.FollowerUserId
        });

        return result;
    }

    public Task<string> RejectFollowRequest(ProcessFollowRequestModel model)
    {
        return contactsService.RejectFollowRequest(model);
    }

    public Task<string> DeleteFollowRequest(SendFollowModel model)
    {
        return contactsService.DeleteFollowRequest(model);
    }

    public Task<string> DeleteFollower(ProcessFollowRequestModel model)
    {
        return contactsService.DeleteFollower(model);
    }

    public Task<string> UnfollowUser(SendFollowModel model)
    {
        return contactsService.UnfollowUser(model);
    }

    #endregion

    #region Settings

    public Task<SettingsModel> GetSettings(string userId)
    {
        return settingsService.GetSettings(userId);
    }

    public Task<SettingsModel> CreateSettings(CreateSettingsModel model)
    {
        return settingsService.CreateSettings(model);
    }

    public Task<SettingsModel> UpdateSettings(UpdateSettingsModel model)
    {
        return settingsService.UpdateSettings(model);
    }

    public Task<string> DeleteSettings(string userId)
    {
        return settingsService.DeleteSettings(userId);
    }

    #endregion

    #region UserProfile

    public Task<UserProfileModel> GetUserProfile(GetUserProfileModel model)
    {
        return userProfileService.GetUserProfile(model.ContactsUserId);
    }

    public Task<UserProfileModel> CreateUserProfile(CreateUserProfileModel model)
    {
        return userProfileService.CreateUserProfile(model);
    }

    public Task<UserProfileModel> UpdateUserProfile(UpdateUserProfileModel model)
    {
        return userProfileService.UpdateUserProfile(model);
    }

    public Task<string> DeleteUserProfile(string userId)
    {
        return userProfileService.DeleteUserProfile(userId);
    }

    #endregion

    #region Food

    public Task<UserFoodsModel> GetFoods(GetUserFoodsModel model)
    {
        return foodService.GetFoods(model);
    }

    public async Task<FoodItemModel> AddFood(AddUserFoodModel model)
    {
        return await foodService.AddFood(model);
    }

    public async Task<FoodItemModel> EditFood(UpdateUserFoodModel model)
    {
        return await foodService.EditFood(model);
    }

    public async Task<string> RemoveFood(string userId, string foodId)
    {
        return await foodService.RemoveFood(userId, foodId);
    }

    #endregion

    #region Exercises

    public async Task<UserExercisesModel> GetExercises(GetUserExercisesModel model)
    {
        return await exercisesService.GetExercises(model);
    }

    public async Task<ExerciseItemModel> AddExercise(AddUserExerciseModel model)
    {
        return await exercisesService.AddExercise(model);
    }

    public async Task<ExerciseItemModel> EditExercise(UpdateUserExerciseModel model)
    {
        return await exercisesService.EditExercise(model);
    }

    public async Task<string> RemoveExercise(string userId, string exerciseId)
    {
        return await exercisesService.RemoveExercise(userId, exerciseId);
    }

    #endregion

    #region Notification

    public Task<string> GetNotificationTicket(NotificationTicketModel model)
    {
        return notificationService.GetNotificationTicket(model);
    }

    public Task<bool> ValidateNotificationTicket(ValidateNotificationTicketModel model)
    {
        return notificationService.ValidateNotificationTicket(model);
    }

    #endregion
}