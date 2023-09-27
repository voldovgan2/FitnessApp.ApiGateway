using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Enums.Contacts;
using FitnessApp.ApiGateway.Enums.Settings;
using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Contacts.Output;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.ApiGateway.Services.Settings;

namespace FitnessApp.ApiGateway.Services.Contacts
{
    public class ContactsService : IContactsService
    {
        private const string GET_USER_CONTACTS_METHOD = "GetUserContacts";
        private const string GET_USER_CONTACTS_COUNT_METHOD = "GetUserContactsCount";
        private const string GET_IS_FOLLOWER_METHOD = "GetIsFollower";
        private const string START_FOLLOW_METHOD = "StartFollow";
        private const string ACCEPT_FOLLOW_REQUEST_METHOD = "AcceptFollowRequest";
        private const string REJECT_FOLLOW_REQUEST_METHOD = "RejectFollowRequest";
        private const string DELETE_FOLLOW_REQUEST_METHOD = "DeleteFollowRequest";
        private const string DELETE_FOLLOWER_METHOD = "DeleteFollower";
        private const string UNFOLLOW_USER_METHOD = "UnfollowUser";

        private readonly IInternalClient _internalClient;
        private readonly ApiClientSettings _apiClientSettings;
        private readonly ISettingsService _settingsService;
        private readonly AuthenticationTokenRequest _authenticationTokenRequest;

        public ContactsService(
            ApiClientSettings apiClientSettings,
            ISettingsService settingsService,
            IInternalClient internalClient)
        {
            _apiClientSettings = apiClientSettings;
            _settingsService = settingsService;
            _internalClient = internalClient;
            _authenticationTokenRequest = new AuthenticationTokenRequest
            {
                ApiName = _apiClientSettings.ApiName,
                Scope = _apiClientSettings.Scope
            };
        }

        public async Task<bool> CanViewUserContacts(GetUserContactsModel model)
        {
            var result = model.ContactsUserId == model.UserId;
            if (!result)
            {
                var settings = await _settingsService.GetSettings(model.ContactsUserId);
                if (settings != null)
                {
                    result =
                        (model.ContactsType == ContactsType.Followers
                            && ((settings.CanViewFollowers == PrivacyType.All) || (settings.CanViewFollowers == PrivacyType.Followers && await IsFollower(model))))
                        || (model.ContactsType == ContactsType.Followings
                            && ((settings.CanViewFollowings == PrivacyType.All) || (settings.CanViewFollowings == PrivacyType.Followers && await IsFollower(model))));
                }
            }

            return result;
        }

        public async Task<IEnumerable<ContactModel>> GetUserContacts(GetUserContactsModel model)
        {
            var payload = new
            {
                UserId = model.ContactsUserId,
                model.ContactsType
            };
            var request = new InternalRequest(
                HttpMethod.Get,
                _apiClientSettings.Url,
                _apiClientSettings.ApiName,
                GET_USER_CONTACTS_METHOD,
                null,
                null,
                payload);
            var result = await _internalClient.SendInternalRequest<IEnumerable<ContactModel>>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<UserContactsCountModel> GetUserContactsCount(string userId)
        {
            var request = new InternalRequest(
                HttpMethod.Get,
                _apiClientSettings.Url,
                _apiClientSettings.ApiName,
                GET_USER_CONTACTS_COUNT_METHOD,
                new string[] { userId },
                null,
                null);
            var result = await _internalClient.SendInternalRequest<UserContactsCountModel>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<bool> IsFollower(GetUserContactsModel model)
        {
            var request = new InternalRequest(
                HttpMethod.Get,
                _apiClientSettings.Url,
                _apiClientSettings.ApiName,
                GET_IS_FOLLOWER_METHOD,
                null,
                null,
                model);
            var result = await _internalClient.SendInternalRequest<bool>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<string> StartFollow(SendFollowModel model)
        {
            var request = new InternalRequest(
                HttpMethod.Post,
                _apiClientSettings.Url,
                _apiClientSettings.ApiName,
                START_FOLLOW_METHOD,
                null,
                model,
                null);
            var result = await _internalClient.SendInternalRequest<string>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<string> AcceptFollowRequest(ProcessFollowRequestModel model)
        {
            var request = new InternalRequest(
                HttpMethod.Post,
                _apiClientSettings.Url,
                _apiClientSettings.ApiName,
                ACCEPT_FOLLOW_REQUEST_METHOD,
                null,
                model,
                null);
            var result = await _internalClient.SendInternalRequest<string>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<string> RejectFollowRequest(ProcessFollowRequestModel model)
        {
            var request = new InternalRequest(
                HttpMethod.Post,
                _apiClientSettings.Url,
                _apiClientSettings.ApiName,
                REJECT_FOLLOW_REQUEST_METHOD,
                null,
                model,
                null);
            var result = await _internalClient.SendInternalRequest<string>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<string> DeleteFollowRequest(SendFollowModel model)
        {
            var request = new InternalRequest(
                HttpMethod.Post,
                _apiClientSettings.Url,
                _apiClientSettings.ApiName,
                DELETE_FOLLOW_REQUEST_METHOD,
                null,
                model,
                null);
            var result = await _internalClient.SendInternalRequest<string>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<string> DeleteFollower(ProcessFollowRequestModel model)
        {
            var request = new InternalRequest(
                HttpMethod.Post,
                _apiClientSettings.Url,
                _apiClientSettings.ApiName,
                DELETE_FOLLOWER_METHOD,
                null,
                model,
                null
            );
            var result = await _internalClient.SendInternalRequest<string>(_authenticationTokenRequest, request);
            return result;
        }

        public async Task<string> UnfollowUser(SendFollowModel model)
        {
            var request = new InternalRequest(
                HttpMethod.Post,
                _apiClientSettings.Url,
                _apiClientSettings.ApiName,
                UNFOLLOW_USER_METHOD,
                null,
                model,
                null
            );
            var result = await _internalClient.SendInternalRequest<string>(_authenticationTokenRequest, request);
            return result;
        }
    }
}