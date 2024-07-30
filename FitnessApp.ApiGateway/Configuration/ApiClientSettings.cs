using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Configuration;

[ExcludeFromCodeCoverage]
public class ApiClientSettings
{
    public string ApiName { get; set; }
    public string Url { get; set; }
    public string Scope { get; set; }
}
