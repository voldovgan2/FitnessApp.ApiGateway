using System.Linq;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Exercises.Input;
using FitnessApp.ApiGateway.Models.Exercises.Output;
using FitnessApp.ApiGateway.Models.Food.Input;
using FitnessApp.ApiGateway.Models.Food.Output;
using FitnessApp.ApiGateway.Models.Settings.Input;
using FitnessApp.ApiGateway.Models.Settings.Output;
using FitnessApp.ApiGateway.Models.SignalR;
using FitnessApp.ApiGateway.Models.UserProfile.Input;
using FitnessApp.ApiGateway.Models.UserProfile.Output;
using FitnessApp.ApiGateway.Services.Contacts;
using FitnessApp.ApiGateway.Services.Exercises;
using FitnessApp.ApiGateway.Services.Food;
using FitnessApp.ApiGateway.Services.Settings;
using FitnessApp.ApiGateway.Services.SignalR;
using FitnessApp.ApiGateway.Services.UserProfile;
using FitnessApp.Common.Paged.Extensions;
using FitnessApp.Common.Paged.Models.Output;

namespace FitnessApp.ApiGateway.Services.Aggregator
{
    public class AggregatorService : IAggregatorService
    {
        private readonly IContactsService _contactsService;
        private readonly ISettingsService _settingsService;
        private readonly IUserProfileService _userProfileService;
        private readonly IFoodService _foodService;
        private readonly IExercisesService _exercisesService;
        private readonly ISignalR _signalR;

        public AggregatorService(
            IContactsService contactsService,
            ISettingsService settingsService,
            IUserProfileService userProfileService,
            IFoodService foodService,
            IExercisesService exercisesService,
            ISignalR signalR)
        {
            _contactsService = contactsService;
            _settingsService = settingsService;
            _userProfileService = userProfileService;
            _foodService = foodService;
            _exercisesService = exercisesService;
            _signalR = signalR;
        }

        #region Contacts

        public Task<bool> CanViewUserContacts(GetUserContactsModel model)
        {
            return _contactsService.CanViewUserContacts(model);
        }

        public async Task<PagedDataModel<UserProfileModel>> GetUserContacts(GetUserContactsModel model)
        {
            PagedDataModel<UserProfileModel> result = null;
            var contacts = await _contactsService.GetUserContacts(model);
            if (contacts != null)
            {
                var getSelectedUsersProfilesModel = new GetSelectedUsersProfilesModel
                {
                    UsersIds = contacts.Select(i => i.UserId)
                };
                var profiles = await _userProfileService.GetUsersProfiles(getSelectedUsersProfilesModel);
                if (profiles != null)
                {
                    foreach (var item in profiles)
                    {
                        item.CanFollow = !await _contactsService.IsFollower(new GetUserContactsModel
                        {
                            UserId = model.UserId,
                            ContactsUserId = item.UserId
                        });
                    }

                    result = profiles.ToPaged(model);
                }
            }

            return result;
        }

        public Task<string> StartFollow(SendFollowModel model)
        {
            return _contactsService.StartFollow(model);
        }

        public async Task<string> AcceptFollowRequest(ProcessFollowRequestModel model)
        {
            var result = await _contactsService.AcceptFollowRequest(model);
            if (result != null)
            {
                await _signalR.SendMessage(new FollowRequestConfirmedModel
                {
                    Sender = model.UserId,
                    Receiver = model.FollowerUserId
                });
            }

            return result;
        }

        public Task<string> RejectFollowRequest(ProcessFollowRequestModel model)
        {
            return _contactsService.RejectFollowRequest(model);
        }

        public Task<string> DeleteFollowRequest(SendFollowModel model)
        {
            return _contactsService.DeleteFollowRequest(model);
        }

        public Task<string> DeleteFollower(ProcessFollowRequestModel model)
        {
            return _contactsService.DeleteFollower(model);
        }

        public Task<string> UnfollowUser(SendFollowModel model)
        {
            return _contactsService.UnfollowUser(model);
        }

        #endregion

        #region Settings

        public Task<SettingsModel> GetSettings(string userId)
        {
            return _settingsService.GetSettings(userId);
        }

        public Task<SettingsModel> CreateSettings(CreateSettingsModel model)
        {
            return _settingsService.CreateSettings(model);
        }

        public Task<SettingsModel> UpdateSettings(UpdateSettingsModel model)
        {
            return _settingsService.UpdateSettings(model);
        }

        public Task<string> DeleteSettings(string userId)
        {
            return _settingsService.DeleteSettings(userId);
        }

        #endregion

        #region UserProfile

        public Task<UserProfileModel> GetUserProfile(GetUserProfileModel model)
        {
            return _userProfileService.GetUserProfile(model.ContactsUserId);
        }

        public Task<UserProfileModel> CreateUserProfile(CreateUserProfileModel model)
        {
            return _userProfileService.CreateUserProfile(model);
        }

        public Task<UserProfileModel> UpdateUserProfile(UpdateUserProfileModel model)
        {
            return _userProfileService.UpdateUserProfile(model);
        }

        public Task<string> DeleteUserProfile(string userId)
        {
            return _userProfileService.DeleteUserProfile(userId);
        }

        #endregion

        #region Food

        public Task<UserFoodsModel> GetFoods(GetUserFoodsModel model)
        {
            return _foodService.GetFoods(model);
        }

        public async Task<FoodItemModel> AddFood(AddUserFoodModel model)
        {
            return await _foodService.AddFood(model);
        }

        public async Task<FoodItemModel> EditFood(UpdateUserFoodModel model)
        {
            return await _foodService.EditFood(model);
        }

        public async Task<string> RemoveFood(string userId, string foodId)
        {
            return await _foodService.RemoveFood(userId, foodId);
        }

        #endregion

        #region Exercises

        public async Task<UserExercisesModel> GetExercises(GetUserExercisesModel model)
        {
            return await _exercisesService.GetExercises(model);
        }

        public async Task<ExerciseItemModel> AddExercise(AddUserExerciseModel model)
        {
            return await _exercisesService.AddExercise(model);
        }

        public async Task<ExerciseItemModel> EditExercise(UpdateUserExerciseModel model)
        {
            return await _exercisesService.EditExercise(model);
        }

        public async Task<string> RemoveExercise(string userId, string exerciseId)
        {
            return await _exercisesService.RemoveExercise(userId, exerciseId);
        }

        #endregion

        #region SignalR

        public Task<string> GetToken()
        {
            return _signalR.GetToken();
        }

        #endregion
    }
}