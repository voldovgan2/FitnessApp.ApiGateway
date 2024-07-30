using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessApp.ApiGateway.Configuration;

[ExcludeFromCodeCoverage]
public static class CorsExtensions
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(o => o.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        }));
        return services;
    }
}
