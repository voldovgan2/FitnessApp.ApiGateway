using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Models.UserProfile.Input;
using FitnessApp.ApiGateway.Models.UserProfile.Output;
using FitnessApp.ApiGateway.Services.Abstractions;
using FitnessApp.ApiGateway.Services.InternalClient;

namespace FitnessApp.ApiGateway.Services.UserProfile
{
    public class UserProfileService(
        ApiClientSettings apiClientSettings,
        IInternalClient internalClient)
        : GenericService<UserProfileModel>(apiClientSettings, internalClient),
        IUserProfileService
    {
        private const string API = "UserProfile";
        private const string GET_USER_PROFILE_METHOD = "GetUserProfile";
        private const string CREATE_USER_PROFILE_METHOD = "CreateUserProfile";
        private const string UPDATE_USER_PROFILE_METHOD = "UpdateUserProfile";
        private const string DELETE_USER_PROFILE_METHOD = "DeleteUserProfile";
        private const string GET_USER_PROFILES_METHOD = "GetUserProfiles";

        public Task<UserProfileModel> GetUserProfile(string userId)
        {
            return GetItem(ApiClientSettings.Url, API, GET_USER_PROFILE_METHOD, userId);
        }

        public Task<UserProfileModel> CreateUserProfile(CreateUserProfileModel model)
        {
            return CreateItem(ApiClientSettings.Url, API, CREATE_USER_PROFILE_METHOD, model);
        }

        public Task<UserProfileModel> UpdateUserProfile(UpdateUserProfileModel model)
        {
            return CreateItem(ApiClientSettings.Url, API, UPDATE_USER_PROFILE_METHOD, model);
        }

        public Task<string> DeleteUserProfile(string userId)
        {
            return DeleteItem(ApiClientSettings.Url, API, DELETE_USER_PROFILE_METHOD, userId);
        }

        public async Task<IEnumerable<UserProfileModel>> GetUsersProfiles(GetSelectedUsersProfilesModel model)
        {
            var request = new InternalRequest(
                HttpMethod.Get,
                ApiClientSettings.Url,
                ApiClientSettings.ApiName,
                GET_USER_PROFILES_METHOD,
                null,
                model,
                null);
            IEnumerable<UserProfileModel> result = await InternalClient.SendInternalRequest<IEnumerable<UserProfileModel>>(null, request);
            return result;
        }
    }
}