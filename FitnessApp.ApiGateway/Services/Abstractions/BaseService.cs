using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Services.InternalClient;

namespace FitnessApp.ApiGateway.Services.Abstractions
{
    public abstract class BaseService(ApiClientSettings apiClientSettings, IInternalClient internalClient)
    {
        protected IInternalClient InternalClient { get; } = internalClient;
        protected ApiClientSettings ApiClientSettings { get; } = apiClientSettings;

        protected AuthenticationTokenRequest CreateAuthenticationTokenRequest()
        {
            return new AuthenticationTokenRequest
            {
                ApiName = ApiClientSettings.ApiName,
                Scope = ApiClientSettings.Scope
            };
        }
    }
}
