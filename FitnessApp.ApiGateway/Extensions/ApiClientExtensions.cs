using FitnessApp.ApiGateway.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessApp.ApiGateway.Extensions;

public static class ApiClientExtensions
{
    public static IServiceCollection AddBaseApiClient(this IServiceCollection services, string apiName, IConfiguration configuration)
    {
        services.Configure<ApiClientSettings>(apiName, configuration.GetSection($"Apis:{apiName}"));
        return services;
    }
}