using FitnessApp.ApiGateway.Services.Authorization;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.Common.ServiceBus.Nats.Services;
using FitnessApp.Common.Vault;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace FitnessApp.ApiGateway.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureTestServices(services =>
            {
                services
                    .AddAuthentication(defaultScheme: MockConstants.Scheme)
                    .AddScheme<AuthenticationSchemeOptions, MockAuthenticationHandler>(MockConstants.Scheme, options => { });

                services.RemoveAll<IVaultService>();
                services.AddSingleton<IVaultService, MockVaultService>();

                services.RemoveAll<IServiceBus>();
                services.AddSingleton<IServiceBus, MockServiceBus>();

                services.RemoveAll<ITokenClient>();
                services.AddSingleton<ITokenClient, MockTokenClient>();

                services.RemoveAll<IInternalClient>();
                services.AddSingleton<IInternalClient, MockInternalClient>();

                services.RemoveAll<IDistributedCache>();
                services.AddSingleton(Options.Create(new MemoryDistributedCacheOptions()));
                services.AddSingleton<IDistributedCache, MemoryDistributedCache>();
            })
            .UseEnvironment("Development");
    }
}
