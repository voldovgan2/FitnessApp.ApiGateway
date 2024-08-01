using System.Security.Claims;
using System.Text.Encodings.Web;
using FitnessApp.Common.IntegrationTests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FitnessApp.ApiGateway.IntegrationTests;

public class MockAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : MockAuthenticationHandlerBase(options, logger, encoder)
{
    protected override List<Claim> GetClaimsByRequest(string path)
    {
        return [new Claim("Permission", "Помножувати")];
    }
}
