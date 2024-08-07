﻿using FitnessApp.ApiGateway.Services.Authorization;
using FitnessApp.Common.IntegrationTests;

namespace FitnessApp.ApiGateway.IntegrationTests;

public class MockTokenClient : ITokenClient
{
    public Task<(string AccessToken, int ExpiresIn)> GetAuthenticationToken(string address, string clientId, string clientSecret, string scope)
    {
        return Task.FromResult((MockConstants.SvTest, 3600));
    }
}
