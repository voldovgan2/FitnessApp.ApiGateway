using FitnessApp.ApiGateway.Services.Authorization;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.Common.IntegrationTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace FitnessApp.ApiGateway.IntegrationTests;

public class TestWebApplicationFactory : TestWebApplicationFactoryBase<Program, MockAuthenticationHandler>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder
            .ConfigureTestServices(services =>
            {
                services.RemoveAll<ITokenClient>();
                services.AddSingleton<ITokenClient, MockTokenClient>();

                services.RemoveAll<IInternalClient>();
                services.AddSingleton<IInternalClient, MockInternalClient>();

                services.RemoveAll<IDistributedCache>();
                services.AddSingleton(Options.Create(new MemoryDistributedCacheOptions()));
                services.AddSingleton<IDistributedCache, MemoryDistributedCache>();
            });
    }
}
