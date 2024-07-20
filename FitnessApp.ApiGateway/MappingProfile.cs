using AutoMapper;
using FitnessApp.ApiGateway.Contracts.Contacts.Input;
using FitnessApp.ApiGateway.Contracts.Exercises.Input;
using FitnessApp.ApiGateway.Contracts.Exercises.Output;
using FitnessApp.ApiGateway.Contracts.Food.Input;
using FitnessApp.ApiGateway.Contracts.Food.Output;
using FitnessApp.ApiGateway.Contracts.Settings.Input;
using FitnessApp.ApiGateway.Contracts.Settings.Output;
using FitnessApp.ApiGateway.Contracts.UserProfile.Input;
using FitnessApp.ApiGateway.Contracts.UserProfile.Output;
using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Exercises.Input;
using FitnessApp.ApiGateway.Models.Exercises.Output;
using FitnessApp.ApiGateway.Models.Food.Input;
using FitnessApp.ApiGateway.Models.Food.Output;
using FitnessApp.ApiGateway.Models.Settings.Input;
using FitnessApp.ApiGateway.Models.Settings.Output;
using FitnessApp.ApiGateway.Models.UserProfile.Input;
using FitnessApp.ApiGateway.Models.UserProfile.Output;
using FitnessApp.Common.Paged.Contracts.Output;
using FitnessApp.Common.Paged.Models.Output;

namespace FitnessApp.ApiGateway;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserProfileModel, UsersProfilesShortContract>();
        CreateMap<GetUserContactsContract, GetUserContactsModel>();
        CreateMap<SendFollowContract, SendFollowModel>();
        CreateMap<ProcessFollowRequestContract, ProcessFollowRequestModel>();
        CreateMap<PagedDataModel<UserProfileModel>, PagedDataContract<UsersProfilesShortContract>>();
        CreateMap<PagedDataModel<UsersProfilesShortContract>, PagedDataContract<UserProfileModel>>();

        CreateMap<SettingsModel, SettingsContract>();
        CreateMap<CreateSettingsContract, CreateSettingsModel>();
        CreateMap<UpdateSettingsContract, UpdateSettingsModel>();

        CreateMap<UserProfileModel, UserProfileContract>();
        CreateMap<CreateUserProfileContract, CreateUserProfileModel>();
        CreateMap<UpdateUserProfileContract, UpdateUserProfileModel>();

        CreateMap<GetUserFoodsContract, GetUserFoodsModel>();
        CreateMap<UserFoodsModel, UserFoodsContract>();
        CreateMap<AddUserFoodContract, AddUserFoodModel>();
        CreateMap<UpdateUserFoodContract, UpdateUserFoodModel>();
        CreateMap<FoodItemModel, FoodItemContract>();
        CreateMap<PagedDataModel<FoodItemModel>, PagedDataContract<FoodItemContract>>();

        CreateMap<GetUserExercisesContract, GetUserExercisesModel>();
        CreateMap<UserExercisesModel, UserExercisesContract>();
        CreateMap<AddUserExerciseContract, AddUserExerciseModel>();
        CreateMap<UpdateUserExerciseContract, UpdateUserExerciseModel>();
        CreateMap<ExerciseItemModel, ExerciseItemContract>();
        CreateMap<PagedDataModel<ExerciseItemModel>, PagedDataContract<ExerciseItemContract>>();
    }
}
