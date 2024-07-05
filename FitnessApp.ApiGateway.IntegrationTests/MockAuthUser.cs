using System.Security.Claims;

namespace FitnessApp.ApiGateway.IntegrationTests
{
    public class MockAuthUser(params Claim[] claims)
    {
        public List<Claim> Claims { get; private set; } = claims.ToList();
    }
}
