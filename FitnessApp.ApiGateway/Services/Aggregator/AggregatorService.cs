using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Exercises.Input;
using FitnessApp.ApiGateway.Models.Exercises.Output;
using FitnessApp.ApiGateway.Models.Food.Input;
using FitnessApp.ApiGateway.Models.Food.Output;
using FitnessApp.ApiGateway.Models.Settings.Input;
using FitnessApp.ApiGateway.Models.Settings.Output;
using FitnessApp.ApiGateway.Models.UserProfile.Input;
using FitnessApp.ApiGateway.Models.UserProfile.Output;
using FitnessApp.ApiGateway.Services.Contacts;
using FitnessApp.ApiGateway.Services.Exercises;
using FitnessApp.ApiGateway.Services.Food;
using FitnessApp.ApiGateway.Services.Settings;
using FitnessApp.ApiGateway.Services.UserProfile;
using FitnessApp.Paged.Extensions;
using FitnessApp.Paged.Models.Output;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.Aggregator
{
    public class AggregatorService : IAggregatorService
    {
        private readonly IContactsService _contactsService;
        private readonly ISettingsService<SettingsModel> _settingsService;
        private readonly IUserProfileService<UserProfileModel> _userProfileService;
        private readonly IFoodService<UserFoodsModel, FoodItemModel> _foodService;
        private readonly IExercisesService<UserExercisesModel, ExerciseItemModel> _exercisesService;

        public AggregatorService
        (
            IContactsService contactsService,
            ISettingsService<SettingsModel> settingsService,
            IUserProfileService<UserProfileModel> userProfileService,
            IFoodService<UserFoodsModel, FoodItemModel> foodService,
            IExercisesService<UserExercisesModel, ExerciseItemModel> exercisesService
        )
        {
            _contactsService = contactsService;
            _settingsService = settingsService;
            _userProfileService = userProfileService;
            _foodService = foodService;
            _exercisesService = exercisesService;
        }

        #region Contacts

        public async Task<bool> CanViewUserContactsAsync(GetUserContactsModel model)
        {
            var result = await _contactsService.CanViewUserContactsAsync(model);
            return result;
        }

        public async Task<PagedDataModel<UserProfileModel>> GetUserContactsAsync(GetUserContactsModel model)
        {
            PagedDataModel<UserProfileModel> result = null;
            var contacts = await _contactsService.GetUserContactsAsync(model);
            if (contacts != null)
            {
                var getSelectedUsersProfilesModel = new GetSelectedUsersProfilesModel
                {
                    UsersIds = contacts.Select(i => i.UserId)
                };
                var profiles = await _userProfileService.GetUsersProfilesAsync(getSelectedUsersProfilesModel);
                if (profiles != null)
                {
                    foreach (var item in profiles)
                    {
                        item.CanFollow = !await _contactsService.IsFollowerAsync(new GetUserContactsModel
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

        public async Task<string> StartFollowAsync(SendFollowModel model)
        {
            var result = await _contactsService.StartFollowAsync(model);
            return result;
        }

        public async Task<string> AcceptFollowRequestAsync(ProcessFollowRequestModel model)
        {
            var result = await _contactsService.AcceptFollowRequestAsync(model);
            return result;
        }

        public async Task<string> RejectFollowRequestAsync(ProcessFollowRequestModel model)
        {
            var result = await _contactsService.RejectFollowRequestAsync(model);
            return result;
        }

        public async Task<string> DeleteFollowRequestAsync(SendFollowModel model)
        {
            var result = await _contactsService.DeleteFollowRequestAsync(model);
            return result;
        }

        public async Task<string> DeleteFollowerAsync(ProcessFollowRequestModel model)
        {
            var result = await _contactsService.DeleteFollowerAsync(model);
            return result;
        }

        public async Task<string> UnfollowUserAsync(SendFollowModel model)
        {
            var result = await _contactsService.UnfollowUserAsync(model);
            return result;
        }

        #endregion

        #region Settings

        public async Task<SettingsModel> GetSettingsAsync(string userId)
        {
            return await _settingsService.GetItemAsync(userId);
        }

        public async Task<SettingsModel> CreateSettingsAsync(CreateSettingsModel model)
        {
            return await _settingsService.CreateItemAsync(model);
        }

        public async Task<SettingsModel> UpdateSettingsAsync(UpdateSettingsModel model)
        {
            return await _settingsService.UpdateItemAsync(model);
        }

        public async Task<string> DeleteSettingsAsync(string userId)
        {
            return await _settingsService.DeleteItemAsync(userId);
        }

        #endregion

        #region UserProfile

        public async Task<UserProfileModel> GetUserProfileAsync(GetUserProfileModel model)
        {
            UserProfileModel result =  await _userProfileService.GetItemAsync(model.ContactsUserId);
            if (result != null)
            {
                var userContactsCount = await _contactsService.GetUserContactsCountAsync(model.ContactsUserId);
                if(userContactsCount != null)
                {
                    result.FollowersCount = userContactsCount.FollowersCount;
                    result.FollowingsCount = userContactsCount.FollowingsCount;                    
                }
                if(model.ContactsUserId != model.UserId)
                {
                    result.CanFollow = !await _contactsService.IsFollowerAsync(new GetUserContactsModel
                    {
                        UserId = model.UserId,
                        ContactsUserId = model.ContactsUserId
                    });
                }
            }
            return result;
        }

        public async Task<UserProfileModel> CreateUserProfileAsync(CreateUserProfileModel model)
        {
            return await _userProfileService.CreateItemAsync(model);
        }

        public async Task<UserProfileModel> UpdateUserProfileAsync(UpdateUserProfileModel model)
        {
            return await _userProfileService.UpdateItemAsync(model); 
        }

        public async Task<string> DeleteUserProfileAsync(string userId)
        {
            return await _userProfileService.DeleteItemAsync(userId);
        }

        #endregion

        #region Food 

        public async Task<UserFoodsModel> GetFoodsAsync(GetUserFoodsModel model)
        {
            return await _foodService.GetItemAsync(model);
        }

        public async Task<FoodItemModel> AddFoodAsync(AddUserFoodModel model)
        {
            return await _foodService.AddItemAsync(model);
        }

        public async Task<FoodItemModel> EditFoodAsync(UpdateUserFoodModel model)
        {
            return await _foodService.EditItemAsync(model);
        }

        public async Task<string> RemoveFoodAsync(string userId, string foodId)
        {
            return await _foodService.RemoveItemAsync(userId, foodId);
        }

        #endregion

        #region Exercises 

        public async Task<UserExercisesModel> GetExercisesAsync(GetUserExercisesModel model)
        {
            return await _exercisesService.GetItemAsync(model);
        }

        public async Task<ExerciseItemModel> AddExerciseAsync(AddUserExerciseModel model)
        {
            return await _exercisesService.AddItemAsync(model);
        }

        public async Task<ExerciseItemModel> EditExerciseAsync(UpdateUserExerciseModel model)
        {
            return await _exercisesService.EditItemAsync(model);
        }

        public async Task<string> RemoveExerciseAsync(string userId, string exerciseId)
        {
            return await _exercisesService.RemoveItemAsync(userId, exerciseId);
        }

        #endregion
    }
}