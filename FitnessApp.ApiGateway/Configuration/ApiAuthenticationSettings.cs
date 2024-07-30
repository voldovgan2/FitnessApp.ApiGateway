using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Configuration;

[ExcludeFromCodeCoverage]
public class ApiAuthenticationSettings
{
    public string Address { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}
