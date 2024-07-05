using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Services.Authorization;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.Common.Serializer.JsonSerializer;

namespace FitnessApp.ApiGateway.IntegrationTests
{
    public class MockInternalClient : IInternalClient
    {
        private readonly IJsonSerializer _serializer;
        public MockInternalClient(
            IHttpClientFactory httpClientFactory,
            ITokenProvider tokenProvider,
            IJsonSerializer serializer)
        {
            ArgumentNullException.ThrowIfNull(httpClientFactory);
            ArgumentNullException.ThrowIfNull(tokenProvider);
            ArgumentNullException.ThrowIfNull(serializer);
            _serializer = serializer;
        }

        public Task<TResponse> SendInternalRequest<TResponse>(string apiName, string scope, InternalRequest internalRequest)
        {
            if (typeof(TResponse) == typeof(string))
                return Task.FromResult(_serializer.DeserializeFromString<TResponse>(_serializer.SerializeToString(MockConstants.SvTest)));
            else
                return Task.FromResult(Activator.CreateInstance<TResponse>());
        }
    }
}
