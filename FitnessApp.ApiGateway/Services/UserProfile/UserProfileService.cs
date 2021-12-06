using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Models.UserProfile.Input;
using FitnessApp.ApiGateway.Services.Abstractions.Base;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.Serializer.JsonSerializer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.UserProfile
{
    public class UserProfileService<Model> : GenericService<Model>, IUserProfileService<Model>
    {
        private readonly IInternalClient _internalClient;
        private readonly ApiClientSettings _apiClientSettings;
        private const string _serviceName = "UserProfile";

        public UserProfileService
        (
            IHttpClientFactory httpClientFactory,
            IDistributedCache distributedCache,
            IOptions<AuthenticationSettings> authenticationSettings,
            IOptionsMonitor<ApiClientSettings> optionsMonitor,
            IJsonSerializer serializer,
            ILoggerFactory loggerFactory
        )
            : base(_serviceName, httpClientFactory, distributedCache, authenticationSettings, optionsMonitor, serializer, loggerFactory)
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
        }

        public async Task<IEnumerable<Model>> GetUsersProfilesAsync(GetSelectedUsersProfilesModel model)
        {
            var request = new InternalRequest
            (
                HttpMethod.Get, 
                _apiClientSettings.Url,
                _apiClientSettings.ApiName, 
                "GetUsersProfiles", 
                null, 
                model, 
                null
            );
            IEnumerable<Model> result = await _internalClient.SendInternalRequest<IEnumerable<Model>>(request, CancellationToken.None);            
            return result;
        }
    }
}