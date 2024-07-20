using FitnessApp.ApiGateway.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessApp.ApiGateway.Configuration;

public static class AuthorizationExtensions
{
    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, SvTestRequirementHandler>();
        services.Configure<AuthorizationOptions>(options =>
        {
            options.AddPolicy(
                "svTestPolicy",
                policy => policy.Requirements.Add(new SvTestRequirement()));
        });
        return services;
    }
}
