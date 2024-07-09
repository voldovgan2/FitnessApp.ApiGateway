using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FitnessApp.ApiGateway.IntegrationTests
{
    public class MockAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var identity = new ClaimsIdentity(GetClaimsByRequest(Request.Path), MockConstants.SvTest);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, MockConstants.Scheme);
            var result = AuthenticateResult.Success(ticket);
            return Task.FromResult(result);
        }

        private Claim[] GetClaimsByRequest(string path)
        {
            ArgumentNullException.ThrowIfNull(path);
            return [
                new Claim(ClaimTypes.NameIdentifier, MockConstants.SvTest),
                new Claim("Permission", "ToMultiply")
            ];
        }
    }
}
