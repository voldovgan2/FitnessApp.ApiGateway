using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FitnessApp.ApiGateway.Contracts.Contacts.Input;
using FitnessApp.ApiGateway.Contracts.Exercises.Input;
using FitnessApp.ApiGateway.Contracts.Food.Input;
using FitnessApp.ApiGateway.Contracts.Settings.Input;
using FitnessApp.ApiGateway.Contracts.UserProfile.Input;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FitnessApp.ApiGateway.IntegrationTests
{
    public class AggregatorControllerTest : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public AggregatorControllerTest(TestWebApplicationFactory factory)
        {
            _httpClient = factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: MockConstants.Scheme);
        }

        #region Contacts

        [Fact]
        public async Task GetUserContacts_ReturnsOk()
        {
            // Act
            var response = await _httpClient.GetAsync("api/Aggregator/GetUserContacts?contactsUserId=svTest&contactsType=0");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Settings

        [Fact]
        public async Task GetSettings_ReturnsOk()
        {
            // Act
            var response = await _httpClient.GetAsync("api/Aggregator/GetSettings");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteSettings_ReturnsOk()
        {
            // Act
            var response = await _httpClient.DeleteAsync("api/Aggregator/DeleteSettings");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region User profile

        [Fact]
        public async Task GetUserProfile_ReturnsOk()
        {
            // Act
            var response = await _httpClient.GetAsync("api/Aggregator/GetUserProfile");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetUserProfileById_ReturnsOk()
        {
            // Act
            var response = await _httpClient.GetAsync("api/Aggregator/GetUserProfile/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUserProfile_ReturnsOk()
        {
            // Act
            var response = await _httpClient.DeleteAsync("api/Aggregator/DeleteUserProfile");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Food

        [Fact]
        public async Task GetFood_ReturnsOk()
        {
            // Act
            var response = await _httpClient.GetAsync("api/Aggregator/GetFood");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RemoveFood_ReturnsOk()
        {
            // Act
            var response = await _httpClient.DeleteAsync("api/Aggregator/RemoveFood/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Exercises

        [Fact]
        public async Task GetExercises_ReturnsOk()
        {
            // Act
            var response = await _httpClient.GetAsync("api/Aggregator/GetExercises");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RemoveExercise_ReturnsOk()
        {
            // Act
            var response = await _httpClient.DeleteAsync("api/Aggregator/RemoveExercise/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Notification

        [Fact]
        public async Task GetNotificationTicket_ReturnsOk()
        {
            // Act
            var response = await _httpClient.GetAsync("api/Aggregator/GetNotificationTicket");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ValidateNotificationTicket_ReturnsOk()
        {
            // Act
            var response = await _httpClient.GetAsync("api/Aggregator/ValidateNotificationTicket?ticket=svTest");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion
    }
}
