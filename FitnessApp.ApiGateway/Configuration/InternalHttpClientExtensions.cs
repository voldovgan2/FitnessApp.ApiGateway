using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessApp.ApiGateway.Configuration;

[ExcludeFromCodeCoverage]
public static class InternalHttpClientExtensions
{
    public static IServiceCollection ConfigureInternalHttpClient(this IServiceCollection services)
    {
#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1005 // Single line comments should begin with single space
        services.AddHttpClient("InternalClient", client =>
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
            //.AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
            //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            //.WaitAndRetryAsync(int.Parse(internalApiSection.GetSection("RetryAttempt").Value), retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
            ;
#pragma warning restore SA1005 // Single line comments should begin with single space
#pragma warning restore SA1515 // Single-line comment should be preceded by blank line
        return services;
    }
}
