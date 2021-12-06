using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Enums.Contacts;
using FitnessApp.ApiGateway.Enums.Settings;
using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Contacts.Output;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Models.Settings.Output;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.ApiGateway.Services.Settings;
using FitnessApp.Serializer.JsonSerializer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.Contacts
{
    public class ContactsService : IContactsService
    {
        private readonly IInternalClient _internalClient;
        private readonly ApiClientSettings _apiClientSettings;
        private readonly ISettingsService<SettingsModel> _settingsService;
        private const string _serviceName = "Contacts";

        public ContactsService
        (
            IHttpClientFactory httpClientFactory,
            IDistributedCache distributedCache,
            IOptions<AuthenticationSettings> authenticationSettings,
            IOptionsMonitor<ApiClientSettings> optionsMonitor,
            ISettingsService<SettingsModel> settingsService,
            IJsonSerializer serializer,
            ILoggerFactory loggerFactory
        )
        {
            _apiClientSettings = optionsMonitor.Get(_serviceName);
            _apiClientSettings.ApiName = _serviceName;
            _internalClient = new InternalClient.InternalClient
            (
                httpClientFactory,
                distributedCache,
                authenticationSettings.Value,
                _serviceName,
                _apiClientSettings.Scope,
                serializer, 
                loggerFactory
            );
            _settingsService = settingsService;
        }

        public async Task<bool> CanViewUserContactsAsync(GetUserContactsModel model)
        {
            var result = model.ContactsUserId == model.UserId;
            if (!result)
            {
                var settings = await _settingsService.GetItemAsync(model.ContactsUserId);
                if (settings != null)
                {
                    result =
                        model.ContactsType == ContactsType.Followers
                            && ((settings.CanViewFollowers == PrivacyType.All) || (settings.CanViewFollowers == PrivacyType.Followers && await IsFollowerAsync(model)))
                        || model.ContactsType == ContactsType.Followings
                            && ((settings.CanViewFollowings == PrivacyType.All) || (settings.CanViewFollowings == PrivacyType.Followers && await IsFollowerAsync(model)));
                }
            }
            return result;
        }

        public async Task<IEnumerable<ContactModel>> GetUserContactsAsync(GetUserContactsModel model)
        {
            var remoteModel = new
            {
                UserId = model.ContactsUserId,
                model.ContactsType
            };
            var request = new InternalRequest
            (
                HttpMethod.Get, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                "GetUserContacts", 
                null, 
                null, 
                remoteModel
            );            
            var result = await _internalClient.SendInternalRequest<IEnumerable<ContactModel>>(request, CancellationToken.None);
            return result;
        }

        public async Task<UserContactsCountModel> GetUserContactsCountAsync(string userId)
        {
            var request = new InternalRequest
            (
                HttpMethod.Get, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                "GetUserContactsCount", 
                new string[] { userId }, 
                null, 
                null
            );
            var result = await _internalClient.SendInternalRequest<UserContactsCountModel>(request, CancellationToken.None);
            return result;
        }
        
        public async Task<bool> IsFollowerAsync(GetUserContactsModel model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Get, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                "GetIsFollower", 
                null, 
                null, 
                model
            );
            var result = await _internalClient.SendInternalRequest<bool>(request, CancellationToken.None);
            return result;
        }

        public async Task<string> StartFollowAsync(SendFollowModel model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Post, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                "StartFollow", 
                null, 
                model, 
                null
            );
            var result = await _internalClient.SendInternalRequest<string>(request, CancellationToken.None);
            return result;
        }

        public async Task<string> AcceptFollowRequestAsync(ProcessFollowRequestModel model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Post, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                "AcceptFollowRequest", 
                null, 
                model, 
                null
            );
            var result = await _internalClient.SendInternalRequest<string>(request, CancellationToken.None);
            return result;
        }

        public async Task<string> RejectFollowRequestAsync(ProcessFollowRequestModel model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Post, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                "RejectFollowRequest", 
                null, 
                model, 
                null
            );
            var result = await _internalClient.SendInternalRequest<string>(request, CancellationToken.None);
            return result;
        }

        public async Task<string> DeleteFollowRequestAsync(SendFollowModel model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Post, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                "DeleteFollowRequest", 
                null, 
                model, 
                null
            );
            var result = await _internalClient.SendInternalRequest<string>(request, CancellationToken.None);
            return result;
        }

        public async Task<string> DeleteFollowerAsync(ProcessFollowRequestModel model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Post, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                "DeleteFollower", 
                null, 
                model, 
                null
            );
            var result = await _internalClient.SendInternalRequest<string>(request, CancellationToken.None);
            return result;
        }

        public async Task<string> UnfollowUserAsync(SendFollowModel model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Post, 
                _apiClientSettings.Url, 
                _apiClientSettings.ApiName, 
                "UnfollowUser", 
                null, 
                model, 
                null
            );
            var result = await _internalClient.SendInternalRequest<string>(request, CancellationToken.None);
            return result;
        }
    }
}