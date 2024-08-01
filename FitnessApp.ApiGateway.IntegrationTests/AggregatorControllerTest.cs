using System.Net;
using System.Net.Http.Json;
using FitnessApp.ApiGateway.Contracts.Contacts.Input;
using FitnessApp.ApiGateway.Contracts.Exercises.Input;
using FitnessApp.ApiGateway.Contracts.Exercises.Output;
using FitnessApp.ApiGateway.Contracts.Food.Input;
using FitnessApp.ApiGateway.Contracts.Food.Output;
using FitnessApp.ApiGateway.Contracts.Settings.Input;
using FitnessApp.ApiGateway.Contracts.Settings.Output;
using FitnessApp.ApiGateway.Contracts.UserProfile.Input;
using FitnessApp.ApiGateway.Contracts.UserProfile.Output;
using FitnessApp.Common.IntegrationTests;
using FitnessApp.Common.Paged.Contracts.Output;
using Newtonsoft.Json;

namespace FitnessApp.ApiGateway.IntegrationTests;

public class AggregatorControllerTest : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public AggregatorControllerTest(TestWebApplicationFactory factory)
    {
        _httpClient = factory.CreateHttpClient();
    }

    #region Contacts

    [Fact]
    public async Task GetUserContacts_ReturnsOk()
    {
        // Act
        var response = await _httpClient.GetAsync("api/Aggregator/GetUserContacts?contactsUserId=svTest&contactsType=0&page=0&pageSize=10");
        var responseData = JsonConvert.DeserializeObject<PagedDataContract<UsersProfilesShortContract>>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task StartFollow_ReturnsOk()
    {
        // Arrange
        var sendFollowContract = new SendFollowContract
        {
            UserToFollowId = MockConstants.SvTest,
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/Aggregator/StartFollow", sendFollowContract);
        var responseData = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task AcceptFollowRequest_ReturnsOk()
    {
        // Arrange
        var acceptFollowRequest = new ProcessFollowRequestContract
        {
            UserId = MockConstants.SvTest,
            FollowerUserId = MockConstants.SvTest,
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/Aggregator/acceptFollowRequest", acceptFollowRequest);
        var responseData = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task RejectFollowRequest_ReturnsOk()
    {
        // Arrange
        var rejectFollowRequest = new ProcessFollowRequestContract
        {
            UserId = MockConstants.SvTest,
            FollowerUserId = MockConstants.SvTest,
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/Aggregator/rejectFollowRequest", rejectFollowRequest);
        var responseData = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task DeleteFollowRequest_ReturnsOk()
    {
        // Arrange
        var deleteFollowRequest = new SendFollowContract
        {
            UserToFollowId = MockConstants.SvTest,
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/Aggregator/deleteFollowRequest", deleteFollowRequest);
        var responseData = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task DeleteFollower_ReturnsOk()
    {
        // Arrange
        var deleteFollowerRequest = new ProcessFollowRequestContract
        {
            UserId = MockConstants.SvTest,
            FollowerUserId = MockConstants.SvTest,
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/Aggregator/deleteFollower", deleteFollowerRequest);
        var responseData = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task UnfollowUser_ReturnsOk()
    {
        // Arrange
        var unfollowUserRequest = new SendFollowContract
        {
            UserToFollowId = MockConstants.SvTest,
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/Aggregator/unfollowUser", unfollowUserRequest);
        var responseData = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    #endregion

    #region Settings

    [Fact]
    public async Task GetSettings_ReturnsOk()
    {
        // Act
        var response = await _httpClient.GetAsync("api/Aggregator/GetSettings");
        var responseData = JsonConvert.DeserializeObject<SettingsContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task CreateSettings_ReturnsOk()
    {
        // Arrange
        var createSettingsContract = new CreateSettingsContract
        {
            CanFollow = Enums.Settings.PrivacyType.All,
            CanViewExercises = Enums.Settings.PrivacyType.Followers,
            CanViewFollowers = Enums.Settings.PrivacyType.JustMe,
            CanViewFollowings = Enums.Settings.PrivacyType.All,
            CanViewFood = Enums.Settings.PrivacyType.Followers,
            CanViewJournal = Enums.Settings.PrivacyType.JustMe,
            CanViewProgress = Enums.Settings.PrivacyType.All,
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/Aggregator/CreateSettings", createSettingsContract);
        var responseData = JsonConvert.DeserializeObject<SettingsContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task UpdateSettings_ReturnsOk()
    {
        // Arrange
        var updateSettingsContract = new UpdateSettingsContract
        {
            CanFollow = Enums.Settings.PrivacyType.All,
            CanViewExercises = Enums.Settings.PrivacyType.Followers,
            CanViewFollowers = Enums.Settings.PrivacyType.JustMe,
            CanViewFollowings = Enums.Settings.PrivacyType.All,
            CanViewFood = Enums.Settings.PrivacyType.Followers,
            CanViewJournal = Enums.Settings.PrivacyType.JustMe,
            CanViewProgress = Enums.Settings.PrivacyType.All,
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync("api/Aggregator/UpdateSettings", updateSettingsContract);
        var responseData = JsonConvert.DeserializeObject<SettingsContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task DeleteSettings_ReturnsOk()
    {
        // Act
        var response = await _httpClient.DeleteAsync("api/Aggregator/DeleteSettings");
        var responseData = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    #endregion

    #region User profile

    [Fact]
    public async Task GetUserProfile_ReturnsOk()
    {
        // Act
        var response = await _httpClient.GetAsync("api/Aggregator/GetUserProfile");
        var responseData = JsonConvert.DeserializeObject<UserProfileContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task GetUserProfileById_ReturnsOk()
    {
        // Act
        var response = await _httpClient.GetAsync("api/Aggregator/GetUserProfile/1");
        var responseData = JsonConvert.DeserializeObject<UserProfileContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task CreateUserProfile_ReturnsOk()
    {
        // Arrange
        var createUserProfileContract = new CreateUserProfileContract
        {
            FirstName = "FirstName",
            LastName = "LastName",
            About = "About",
            Language = "Language",
            Email = "Email",
            Gender = Enums.UserProfile.Gender.Male,
            BirthDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            BackgroundPhoto = "BackgroundPhoto",
            CroppedProfilePhoto = "CroppedProfilePhoto",
            OriginalProfilePhoto = "OriginalProfilePhoto",
            Height = 175,
            Weight = 70
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/Aggregator/CreateUserProfile", createUserProfileContract);
        var responseData = JsonConvert.DeserializeObject<UserProfileContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task UpdateUserProfile_ReturnsOk()
    {
        // Arrange
        var updateUserProfileContract = new CreateUserProfileContract
        {
            FirstName = "FirstName",
            LastName = "LastName",
            About = "About",
            Language = "Language",
            Email = "Email",
            Gender = Enums.UserProfile.Gender.Male,
            BirthDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            BackgroundPhoto = "BackgroundPhoto",
            CroppedProfilePhoto = "CroppedProfilePhoto",
            OriginalProfilePhoto = "OriginalProfilePhoto",
            Height = 175,
            Weight = 70
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync("api/Aggregator/UpdateUserProfile", updateUserProfileContract);
        var responseData = JsonConvert.DeserializeObject<UserProfileContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task DeleteUserProfile_ReturnsOk()
    {
        // Act
        var response = await _httpClient.DeleteAsync("api/Aggregator/DeleteUserProfile");
        var responseData = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    #endregion

    #region Food

    [Fact]
    public async Task GetFood_ReturnsOk()
    {
        // Act
        var response = await _httpClient.GetAsync("api/Aggregator/GetFood");
        var responseData = JsonConvert.DeserializeObject<UserFoodsContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task AddFood_ReturnsOk()
    {
        // Arrange
        var addUserFoodContract = new AddUserFoodContract
        {
            Name = "Name",
            Description = "Description",
            Photo = "Photo",
            Calories = 1,
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync("api/Aggregator/AddFood", addUserFoodContract);
        var responseData = JsonConvert.DeserializeObject<FoodItemContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task EditFood_ReturnsOk()
    {
        // Arrange
        var updateUserFoodContract = new UpdateUserFoodContract
        {
            Name = "Name",
            Description = "Description",
            Photo = "Photo",
            Calories = 1,
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync("api/Aggregator/EditFood", updateUserFoodContract);
        var responseData = JsonConvert.DeserializeObject<FoodItemContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task RemoveFood_ReturnsOk()
    {
        // Act
        var response = await _httpClient.DeleteAsync("api/Aggregator/RemoveFood/1");
        var responseData = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    #endregion

    #region Exercises

    [Fact]
    public async Task GetExercises_ReturnsOk()
    {
        // Act
        var response = await _httpClient.GetAsync("api/Aggregator/GetExercises");
        var responseData = JsonConvert.DeserializeObject<UserExercisesContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task AddExercise_ReturnsOk()
    {
        // Arrange
        var addUserExerciseContract = new AddUserExerciseContract
        {
            Name = "Name",
            Description = "Description",
            Photo = "Photo",
            Calories = 1,
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync("api/Aggregator/AddExercise", addUserExerciseContract);
        var responseData = JsonConvert.DeserializeObject<ExerciseItemContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task EditExercise_ReturnsOk()
    {
        // Arrange
        var updateUserExerciseContract = new UpdateUserExerciseContract
        {
            Name = "Name",
            Description = "Description",
            Photo = "Photo",
            Calories = 1,
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync("api/Aggregator/EditExercise", updateUserExerciseContract);
        var responseData = JsonConvert.DeserializeObject<ExerciseItemContract>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task RemoveExercise_ReturnsOk()
    {
        // Act
        var response = await _httpClient.DeleteAsync("api/Aggregator/RemoveExercise/1");
        var responseData = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    #endregion

    #region Notification

    [Fact]
    public async Task GetNotificationTicket_ReturnsOk()
    {
        // Act
        var response = await _httpClient.GetAsync("api/Aggregator/GetNotificationTicket");
        var responseData = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task ValidateNotificationTicket_ReturnsOk()
    {
        // Act
        var response = await _httpClient.GetAsync("api/Aggregator/ValidateNotificationTicket?ticket=svTest");
        var responseData = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseData.ToString());
    }

    #endregion
}
