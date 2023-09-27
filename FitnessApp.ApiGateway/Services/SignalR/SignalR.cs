using System;
using System.Net.Http;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Models.SignalR;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.ApiGateway.Services.TokenClient;

namespace FitnessApp.ApiGateway.Services.SignalR
{
    public class SignalR : ISignalR
    {
        private const string SEND_FOLLOW_REQUEST_CONFIRMED_METHOD = "FollowRequestConfirmed";

        private readonly ApiClientSettings _apiClientSettings;
        private readonly ITokenClient _tokenClient;
        private readonly IInternalClient _internalClient;
        private readonly AuthenticationTokenRequest _authenticationTokenRequest;

        public SignalR(
            ApiClientSettings apiClientSettings,
            ITokenClient tokenClient,
            IInternalClient internalClient)
        {
            _apiClientSettings = apiClientSettings;
            _tokenClient = tokenClient;
            _internalClient = internalClient;
            _authenticationTokenRequest = new AuthenticationTokenRequest
            {
                ApiName = _apiClientSettings.ApiName,
                Scope = _apiClientSettings.Scope
            };
        }

        public Task<string> GetToken()
        {
            return _tokenClient.GetAuthenticationToken(_authenticationTokenRequest);
        }

        public Task SendMessage(FollowRequestConfirmedModel model)
        {
            var request = new InternalRequest(
                HttpMethod.Post,
                _apiClientSettings.Url,
                _apiClientSettings.ApiName,
                SEND_FOLLOW_REQUEST_CONFIRMED_METHOD,
                null,
                null,
                model);
            _internalClient.SendInternalRequest<IAsyncResult>(_authenticationTokenRequest, request);
            return Task.CompletedTask;
        }
    }
}