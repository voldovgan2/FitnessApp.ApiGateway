using FitnessApp.ApiGateway.Contracts.Exercises.Output;
using FitnessApp.ApiGateway.Contracts.Food.Output;
using FitnessApp.ApiGateway.Contracts.Settings.Output;
using FitnessApp.ApiGateway.Contracts.UserProfile.Output;
using FitnessApp.ApiGateway.Models.Contacts.Output;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Models.UserProfile.Output;
using FitnessApp.ApiGateway.Services.Authorization;
using FitnessApp.ApiGateway.Services.InternalClient;
using Newtonsoft.Json;

namespace FitnessApp.ApiGateway.IntegrationTests;

public class MockInternalClient : IInternalClient
{
    public MockInternalClient(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(tokenProvider);
    }

    public Task<TResponse> SendInternalRequest<TResponse>(string apiName, string scope, InternalRequest internalRequest)
    {
        var key = $"{internalRequest.Api}_{internalRequest.Method}";
        return key switch
        {
            "Contacts_GetUserContacts" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(new List<ContactModel>
                        { CreateContactModel() }))!),
            "UserProfile_GetUserProfiles" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(new List<UserProfileModel>
                        { CreateUserProfileModel() }))!),
            "Contacts_GetIsFollower" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(true))!),
            "Contacts_StartFollow" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject("UserId"))!),
            "Contacts_AcceptFollowRequest" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject("UserId"))!),
            "Contacts_RejectFollowRequest" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject("UserId"))!),
            "Contacts_DeleteFollowRequest" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject("UserId"))!),
            "Contacts_DeleteFollower" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject("UserId"))!),
            "Contacts_UnfollowUser" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject("UserId"))!),
            "Settings_GetSettings" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(CreateSettingsContract()))!),
            "Settings_CreateSettings" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(CreateSettingsContract()))!),
            "Settings_UpdateSettings" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(CreateSettingsContract()))!),
            "Settings_DeleteSettings" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject("UserId"))!),
            "UserProfile_GetUserProfile" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(CreateUserProfileContract()))!),
            "UserProfile_CreateUserProfile" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(CreateUserProfileContract()))!),
            "UserProfile_UpdateUserProfile" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(CreateUserProfileContract()))!),
            "UserProfile_DeleteUserProfile" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject("UserId"))!),
            "Food_GetFood" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(CreateUserFoodsContract()))!),
            "Food_AddFood" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(CreateFoodItemContract()))!),
            "Food_EditFood" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(CreateFoodItemContract()))!),
            "Food_RemoveFood" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject("UserId"))!),
            "Exercises_GetExercises" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(CreateUserExercisesContract()))!),
            "Exercises_AddExercise" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(CreateExerciseItemContract()))!),
            "Exercises_EditExercise" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject(CreateExerciseItemContract()))!),
            "Exercises_RemoveExercise" => Task.FromResult(
                JsonConvert.DeserializeObject<TResponse>(
                    JsonConvert.SerializeObject("UserId"))!),
            _ => throw new NotImplementedException(),
        };
    }

    private ContactModel CreateContactModel()
    {
        return new ContactModel
        {
            UserId = "UserId"
        };
    }

    private UserProfileModel CreateUserProfileModel()
    {
        return new UserProfileModel
        {
            Id = "Id",
            UserId = "UserId",
            FirstName = "FirstName",
            LastName = "LastName",
            About = "About",
            Gender = Enums.UserProfile.Gender.Male,
            BirthDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Email = "Email",
            Height = 180,
            Weight = 75,
            Language = "Language",
            CanFollow = false,
            BackgroundPhoto = "BackgroundPhoto",
            CroppedProfilePhoto = "CroppedProfilePhoto",
            OriginalProfilePhoto = "OriginalProfilePhoto",
            FollowersCount = 1,
            FollowingsCount = 1,
        };
    }

    private SettingsContract CreateSettingsContract()
    {
        return new SettingsContract
        {
            CanFollow = Enums.Settings.PrivacyType.Followers,
            CanViewFollowers = Enums.Settings.PrivacyType.Followers,
            CanViewFollowings = Enums.Settings.PrivacyType.Followers,
            CanViewFood = Enums.Settings.PrivacyType.Followers,
            CanViewExercises = Enums.Settings.PrivacyType.Followers,
            CanViewJournal = Enums.Settings.PrivacyType.Followers,
            CanViewProgress = Enums.Settings.PrivacyType.Followers,
        };
    }

    private UserProfileContract CreateUserProfileContract()
    {
        return new UserProfileContract
        {
            FirstName = "FirstName",
            LastName = "LastName",
            About = "About",
            Gender = Enums.UserProfile.Gender.Male,
            BirthDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Height = 178,
            Weight = 70,
            Language = "Language",
            Email = "Email",
            BackgroundPhoto = "BackgroundPhoto",
            OriginalProfilePhoto = "OriginalProfilePhoto",
            CroppedProfilePhoto = "CroppedProfilePhoto",
            CanFollow = false,
            FollowersCount = 1,
            FollowingsCount = 2,
        };
    }

    private UserFoodsContract CreateUserFoodsContract()
    {
        return new UserFoodsContract
        {
            UserId = "UserId",
            Foods = new Common.Paged.Contracts.Output.PagedDataContract<FoodItemContract>
            {
                Page = 0,
                TotalCount = 1,
                Items = new List<FoodItemContract>
                {
                    CreateFoodItemContract()
                }
            }
        };
    }

    private FoodItemContract CreateFoodItemContract()
    {
        return new()
        {
            Id = "Id",
            AddedDate = new DateTime(2020, 1, 1, 1, 1, 1, DateTimeKind.Utc),
            Name = "Name",
            Description = "Description",
            Calories = 1,
            Photo = "Photo"
        };
    }

    private UserExercisesContract CreateUserExercisesContract()
    {
        return new UserExercisesContract
        {
            UserId = "UserId",
            Exercises = new Common.Paged.Contracts.Output.PagedDataContract<ExerciseItemContract>
            {
                Page = 0,
                TotalCount = 1,
                Items = new List<ExerciseItemContract>
                {
                    CreateExerciseItemContract()
                }
            }
        };
    }

    private ExerciseItemContract CreateExerciseItemContract()
    {
        return new()
        {
            Id = "Id",
            AddedDate = new DateTime(2020, 1, 1, 1, 1, 1, DateTimeKind.Utc),
            Name = "Name",
            Description = "Description",
            Calories = 1,
            Photo = "Photo"
        };
    }
}
