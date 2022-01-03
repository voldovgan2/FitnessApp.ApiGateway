using FitnessApp.ApiGateway.Models.Blob.Output;
using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Exercises.Input;
using FitnessApp.ApiGateway.Models.Exercises.Output;
using FitnessApp.ApiGateway.Models.Food.Input;
using FitnessApp.ApiGateway.Models.Food.Output;
using FitnessApp.ApiGateway.Models.Settings.Input;
using FitnessApp.ApiGateway.Models.Settings.Output;
using FitnessApp.ApiGateway.Models.UserProfile.Input;
using FitnessApp.ApiGateway.Models.UserProfile.Output;
using FitnessApp.ApiGateway.Services.Blob;
using FitnessApp.ApiGateway.Services.Contacts;
using FitnessApp.ApiGateway.Services.Exercises;
using FitnessApp.ApiGateway.Services.Food;
using FitnessApp.ApiGateway.Services.Settings;
using FitnessApp.ApiGateway.Services.UserProfile;
using FitnessApp.Paged.Extensions;
using FitnessApp.Paged.Models.Output;
using System;
using System.IO;
using System.Linq;
using System.Text;
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
        private readonly IBlobService _blobService;
        private const string PROFILE_CONTAINER_NAME = "Profile";
        private const string FOOD_CONTAINER_NAME = "Food";
        private const string EXERCICES_CONTAINER_NAME = "Exercise";

        public AggregatorService
        (
            IContactsService contactsService,
            ISettingsService<SettingsModel> settingsService,
            IUserProfileService<UserProfileModel> userProfileService,
            IFoodService<UserFoodsModel, FoodItemModel> foodService,
            IExercisesService<UserExercisesModel, ExerciseItemModel> exercisesService,
            IBlobService blobService
        )
        {
            _contactsService = contactsService;
            _settingsService = settingsService;
            _userProfileService = userProfileService;
            _foodService = foodService;
            _exercisesService = exercisesService;
            _blobService = blobService;
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
            var result =  await _userProfileService.GetItemAsync(model.ContactsUserId);
            if (result != null)
            {
                await FillBlobFields
                (
                    result,
                    PROFILE_CONTAINER_NAME, 
                    result.UserId, 
                    new string[]
                    {
                        nameof(UserProfileModel.CroppedProfilePhoto),
                        nameof(UserProfileModel.OriginalProfilePhoto),
                        nameof(UserProfileModel.BackgroundPhoto),
                    }
                );
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
            var result = await ExecuteUpsertOperation
            (
                new
                {
                    model.Email,
                    model.FirstName,
                    model.LastName,
                    model.BirthDate,
                    model.Height,
                    model.Weight,
                    model.Gender,
                    model.About,
                    model.Language
                },
                PROFILE_CONTAINER_NAME,
                new Tuple<string, string>[] 
                {
                    new Tuple<string, string>(nameof(UserProfileModel.CroppedProfilePhoto), model.CroppedProfilePhoto),
                    new Tuple<string, string>(nameof(UserProfileModel.OriginalProfilePhoto), model.OriginalProfilePhoto),
                    new Tuple<string, string>(nameof(UserProfileModel.BackgroundPhoto), model.BackgroundPhoto),
                },
                _userProfileService.CreateItemAsync
            );
            return result;
        }

        public async Task<UserProfileModel> UpdateUserProfileAsync(UpdateUserProfileModel model)
        {
            var result = await ExecuteUpsertOperation
            (
                new
                {
                    model.Email,
                    model.FirstName,
                    model.LastName,
                    model.BirthDate,
                    model.Height,
                    model.Weight,
                    model.Gender,
                    model.About,
                    model.Language
                },
                PROFILE_CONTAINER_NAME,
                new Tuple<string, string>[]
                {
                    new Tuple<string, string>(nameof(UserProfileModel.CroppedProfilePhoto), model.CroppedProfilePhoto),
                    new Tuple<string, string>(nameof(UserProfileModel.OriginalProfilePhoto), model.OriginalProfilePhoto),
                    new Tuple<string, string>(nameof(UserProfileModel.BackgroundPhoto), model.BackgroundPhoto),
                },
                _userProfileService.UpdateItemAsync
            );
            return result;
        }

        public async Task<string> DeleteUserProfileAsync(string userId)
        {
            var result = await _userProfileService.DeleteItemAsync(userId);
            if (!string.IsNullOrEmpty(result))
            {
                await DeleteBlob
                (
                    PROFILE_CONTAINER_NAME,
                    userId,
                    new string[]
                    {
                    nameof(UserProfileModel.CroppedProfilePhoto),
                    nameof(UserProfileModel.OriginalProfilePhoto),
                    nameof(UserProfileModel.BackgroundPhoto),
                    }
                );
            }
            return result;
        }

        #endregion

        #region Food 

        public async Task<UserFoodsModel> GetFoodsAsync(GetUserFoodsModel model)
        {
            var result = await _foodService.GetItemAsync(model);
            if (result != null)
            {
                foreach(var food in result.Foods.Items)
                {
                    await FillBlobFields
                    (
                        food,
                        FOOD_CONTAINER_NAME,
                        food.Id,
                        new string[]
                        {
                            nameof(FoodItemModel.Photo),
                        }
                    );
                }
            }
            return result;
        }

        public async Task<FoodItemModel> AddFoodAsync(AddUserFoodModel model)
        {
            var result = await ExecuteUpsertOperation
            (
                new
                {
                    model.UserId,
                    model.Name,
                    model.Description,
                    model.Calories
                },
                FOOD_CONTAINER_NAME,
                new Tuple<string, string>[]
                {
                    new Tuple<string, string>(nameof(FoodItemModel.Photo), model.Photo)
                },
                _foodService.AddItemAsync
            );
            return result;
        }

        public async Task<FoodItemModel> EditFoodAsync(UpdateUserFoodModel model)
        {
            var result = await ExecuteUpsertOperation
            (
                new
                {
                    model.UserId,
                    model.Name,
                    model.Description,
                    model.Calories
                },
                FOOD_CONTAINER_NAME,
                new Tuple<string, string>[]
                {
                    new Tuple<string, string>(nameof(FoodItemModel.Photo), model.Photo)
                },
                _foodService.EditItemAsync
            );
            return result;
        }

        public async Task<string> RemoveFoodAsync(string userId, string foodId)
        {
            var result = await _foodService.RemoveItemAsync(userId, foodId);
            if (!string.IsNullOrEmpty(result))
            {
                await DeleteBlob
                (
                    FOOD_CONTAINER_NAME,
                    foodId,
                    new string[]
                    {
                        nameof(FoodItemModel.Photo)
                    }
                );
            }
            return result;
        }

        #endregion

        #region Exercises 

        public async Task<UserExercisesModel> GetExercisesAsync(GetUserExercisesModel model)
        {
            var result = await _exercisesService.GetItemAsync(model);
            if (result != null)
            {
                foreach (var exercise in result.Exercises.Items)
                {
                    await FillBlobFields
                    (
                        exercise,
                        EXERCICES_CONTAINER_NAME,
                        exercise.Id,
                        new string[]
                        {
                            nameof(ExerciseItemModel.Photo)
                        }
                    );
                }
            }
            return result;
        }

        public async Task<ExerciseItemModel> AddExerciseAsync(AddUserExerciseModel model)
        {
            var result = await ExecuteUpsertOperation
            (
                new
                {
                    model.UserId,
                    model.Name,
                    model.Description,
                    model.Calories
                },
                EXERCICES_CONTAINER_NAME,
                new Tuple<string, string>[]
                {
                    new Tuple<string, string>(nameof(ExerciseItemModel.Photo), model.Photo)
                },
                _exercisesService.AddItemAsync
            );
            return result;
        }

        public async Task<ExerciseItemModel> EditExerciseAsync(UpdateUserExerciseModel model)
        {
            var result = await ExecuteUpsertOperation
            (
                new
                {
                    model.UserId,
                    model.Name,
                    model.Description,
                    model.Calories
                },
                EXERCICES_CONTAINER_NAME,
                new Tuple<string, string>[]
                {
                    new Tuple<string, string>(nameof(ExerciseItemModel.Photo), model.Photo)
                },
                _exercisesService.EditItemAsync
            );
            return result;
        }

        public async Task<string> RemoveExerciseAsync(string userId, string exerciseId)
        {
            var result = await _exercisesService.RemoveItemAsync(userId, exerciseId);
            if (!string.IsNullOrEmpty(result))
            {
                await DeleteBlob
                (
                    EXERCICES_CONTAINER_NAME,
                    exerciseId,
                    new string[]
                    {
                        nameof(ExerciseItemModel.Photo)
                    }
                );
            }
            return result;
        }

        #endregion

        private async Task<Model> ExecuteUpsertOperation<Model>
        (
            object data, 
            string blobContainerName, 
            Tuple<string, string>[] blobFields, 
            Func<object, Task<Model>> executeService
        )
            where Model: IBlobModel
        {
            var upsertResult = await executeService(data);
            foreach(var blobField in blobFields)
            {
                var blobContent = Encoding.Default.GetBytes(blobField.Item2);
                await _blobService.UploadFile(blobContainerName, CreateBlobName(blobField.Item1, upsertResult.Id), new MemoryStream(blobContent));
                var propertyInfo = typeof(Model).GetProperty(blobField.Item1);
                propertyInfo.SetValue(upsertResult, blobField.Item2);
            }
            return upsertResult;
        }

        private async Task FillBlobFields<Model>
        (
            Model data,
            string blobContainerName,
            string objectId,
            string[] blobFields
        )
            where Model : IBlobModel
        {
            foreach (var blobField in blobFields)
            {
                var blobContent = await _blobService.DownloadFile(blobContainerName, CreateBlobName(blobField, objectId)); 
                var blobValue = new byte[blobContent.Length];
                await blobContent.ReadAsync(blobValue, 0, (int)blobContent.Length);
                var propertyInfo = typeof(Model).GetProperty(blobField);
                propertyInfo.SetValue(data, blobValue);
            }
        }

        private async Task DeleteBlob
        (
            string blobContainerName,
            string objectId,
            string[] blobFields
        )
        {
            foreach (var blobField in blobFields)
            {
                await _blobService.DeleteFile(blobContainerName, CreateBlobName(blobField, objectId));
            }
        }

        private string CreateBlobName(string propertyName, string objectId)
        {
            return $"{propertyName}_{objectId}";
        }
    }
}